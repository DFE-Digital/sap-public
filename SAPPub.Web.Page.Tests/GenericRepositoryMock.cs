using Moq;
using SAPPub.Core.Interfaces.Repositories.Generic;

namespace SAPPub.Web.Page.Tests;

public class GenericRepositoryMock<T> : Mock<IGenericRepository<T>> where T : class
{
}
