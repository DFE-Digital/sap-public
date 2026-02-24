using Lucene.Net.Documents;
using Lucene.Net.Util;
using SAPPub.Core.Entities;
using SAPPub.Infrastructure.LuceneSearch;

public class LuceneIndexWriter
{
    private LuceneIndexContext _context;

    public LuceneIndexWriter(LuceneIndexContext context)
    {
        _context = context;
        _context.Writer.DeleteAll();
    }

    private static readonly FieldType TermVectorFieldType = new FieldType(TextField.TYPE_STORED)
    {
        StoreTermVectors = true,
        StoreTermVectorPositions = true,
        StoreTermVectorOffsets = true,
        StoreTermVectorPayloads = true,
        IsStored = true
    };

    public void AddToIndex(IEnumerable<Establishment> schools)
    {
        foreach (var e in schools)
        {
            var doc = new Document
            {
                new StringField(nameof(Establishment.URN), e.URN.ToString(), Field.Store.YES),
                new Field(nameof(Establishment.EstablishmentName), e.EstablishmentName, TermVectorFieldType),
                new SortedDocValuesField("EstablishmentNameSort", new BytesRef(e.EstablishmentName))
            };
            if (e.GenderName is not null) doc.Add(new StoredField(nameof(Establishment.GenderName), e.GenderName));
            if (e.ReligiousCharacterName is not null) doc.Add(new StoredField(nameof(Establishment.ReligiousCharacterName), e.ReligiousCharacterName));
            if (e.Address is not null) doc.Add(new StoredField(nameof(Establishment.Address), e.Address));
            if (e.AddressPostcode is not null) doc.Add(new StoredField(nameof(Establishment.AddressPostcode), e.AddressPostcode));

            var latlon = MappingHelper.ConvertToLongLat(e.Easting, e.Northing);
            if (latlon != null)
            {
                var point = _context.SpatialContext.MakePoint(latlon.Longitude, latlon.Latitude);
                foreach (var f in _context.GeoStrategy.CreateIndexableFields(point))
                    doc.Add(f);

                doc.Add(new StoredField("lat", latlon!.Latitude));
                doc.Add(new StoredField("lon", latlon.Longitude));
            }

            _context.Writer.AddDocument(doc);
        }
    }

    public void FinaliseIndex()
    {
        _context.Writer.Flush(triggerMerge: true, applyAllDeletes: true);

        _context.SearcherManager.MaybeRefreshBlocking();
    }
}