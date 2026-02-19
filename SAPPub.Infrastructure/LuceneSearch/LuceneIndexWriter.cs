using Lucene.Net.Documents;
using Lucene.Net.Util;
using SAPPub.Core.Entities;
using SAPPub.Infrastructure.LuceneSearch;

public class LuceneIndexWriter(LuceneIndexContext context)
{
    private static readonly FieldType TermVectorFieldType = new FieldType(TextField.TYPE_STORED)
    {
        StoreTermVectors = true,
        StoreTermVectorPositions = true,
        StoreTermVectorOffsets = true,
        StoreTermVectorPayloads = true,
        IsStored = true
    };

    public void BuildIndex(IEnumerable<Establishment> schools)
    {
        context.Writer.DeleteAll();

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
            context.Writer.AddDocument(doc);
        }

        context.Writer.Flush(triggerMerge: true, applyAllDeletes: true);

        context.SearcherManager.MaybeRefreshBlocking();
    }
}