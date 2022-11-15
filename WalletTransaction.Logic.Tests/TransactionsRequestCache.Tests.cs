using Microsoft.VisualStudio.TestPlatform.ObjectModel;

using WalletTransaction.Logic;

namespace Import.Logic.Tests;
public class TransactionsRequestCacheTests
{
    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can be created.")]
    [Trait("Category", "Unit")]
    public void CanBeCreated()
    {
        // Act
        var exception = Record.Exception(() =>
            _ = new TransactionsRequestCache());

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can add single {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanAddSingleTransactionRequest()
    {
        // Arrange
        var transactionsRequestCache = new TransactionsRequestCache();
        var request = CreateStubRequest(1);

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.Add(request));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} cannot add null {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanNotAddNull()
    {
        // Arrange
        var transactionsRequestCache = new TransactionsRequestCache();

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.Add(entity: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} cannot add already exsisting {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanNotAddExsistingTransactionRequest()
    {
        // Arrange
        var request1 = CreateStubRequest(1);
        var request2 = CreateStubRequest(1);
        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.Add(request1);

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.Add(request2));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can add {nameof(TransactionRequest)} range.")]
    [Trait("Category", "Unit")]
    public void CanAddTransactionRequestRange()
    {
        // Arrange
        var requests = new TransactionRequest[] 
        {
            CreateStubRequest(1),
            CreateStubRequest(2),
            CreateStubRequest(3),
        };

        var transactionsRequestCache = new TransactionsRequestCache();

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.AddRange(requests));

        // Assert
        exception.Should().BeNull();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} cannot add null {nameof(TransactionRequest)} range.")]
    [Trait("Category", "Unit")]
    public void CanNotAddNullRange()
    {
        // Arrange
        var transactionsRequestCache = new TransactionsRequestCache();

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.AddRange(entities: null!));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can find {nameof(TransactionRequest)} by key.")]
    [Trait("Category", "Unit")]
    public void CanFindTransactionRequestByKey()
    {
        // Arrange
        var keys = new InternalID[]
        {
            new(1),
            new(2),
            new(4),
        };

        var noneKeys = new InternalID[]
        {
            new(3),
            new(5)
        };

        var requests = new TransactionRequest[]
        {
            CreateStubRequest(1),
            CreateStubRequest(2),
            CreateStubRequest(4),
        };
        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests);

        // Act
        var goodResult = keys.Select(x => transactionsRequestCache.Contains(x));
        var badResult = noneKeys.Select(x => transactionsRequestCache.Contains(x));

        // Assert
        goodResult.Should().AllBeEquivalentTo(true);
        badResult.Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can find {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanFindTransactionRequest()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(1),
            CreateStubRequest(2),
            CreateStubRequest(4),
        };
        var badRequests = new TransactionRequest[]
        {
            CreateStubRequest(3),
            CreateStubRequest(5)
        };

        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests);

        // Act
        var resultGood = requests.Select(x => transactionsRequestCache.Contains(x));
        var resultBad = badRequests.Select(x => transactionsRequestCache.Contains(x));

        // Assert
        resultGood.Should().AllBeEquivalentTo(true);
        resultBad.Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can not find null {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanNotFindNullTransactionRequest()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(1),
            CreateStubRequest(2),
            CreateStubRequest(4),
        };

        TransactionRequest requestFind = null!;
        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests);

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.Contains(requestFind));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can delete {nameof(TransactionRequest)}s.")]
    [Trait("Category", "Unit")]
    public void CanDeleteTransactionRequest()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(3),
            CreateStubRequest(4),
        };

        var deletableRequests = new TransactionRequest[]
        {
            CreateStubRequest(1),
            CreateStubRequest(2)
        };

        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests.Concat(deletableRequests));

        // Act
        transactionsRequestCache.Delete(deletableRequests[0]);
        transactionsRequestCache.Delete(deletableRequests[1]);

        // Assert
        requests.Select(x => transactionsRequestCache.Contains(x)).Should().AllBeEquivalentTo(true);
        deletableRequests.Select(x => transactionsRequestCache.Contains(x)).Should().AllBeEquivalentTo(false);
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can not delete null {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteNullLink()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(5),
            CreateStubRequest(6),
            CreateStubRequest(7),
        };

        TransactionRequest requestFind = null!;
        var TransactionsRequestCache = new TransactionsRequestCache();
        TransactionsRequestCache.AddRange(requests);

        // Act
        var exception = Record.Exception(() =>
            TransactionsRequestCache.Delete(requestFind));

        // Assert
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can not delete not exsisting {nameof(TransactionRequest)}.")]
    [Trait("Category", "Unit")]
    public void CanNotDeleteNotExsistinglLink()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(5),
            CreateStubRequest(6),
            CreateStubRequest(7),
        };

        var transactionRequest = CreateStubRequest(9);
        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests);

        // Act
        var exception = Record.Exception(() =>
            transactionsRequestCache.Delete(transactionRequest));

        // Assert
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can get {nameof(TransactionRequest)} by key.")]
    [Trait("Category", "Unit")]
    public void CanGetByKey()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(5),
            CreateStubRequest(4),
        };

        var keys = new InternalID[]
        {
            new (5),
            new (4),
            new (1)
        };

        var expectedResult = new TransactionRequest?[]
        {
            requests[0],
            CreateStubRequest(4),
            null!
        };

        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests);

        // Act
        var result = keys.Select(x => transactionsRequestCache.GetByKey(x));

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt => opt.WithoutStrictOrdering());
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can cancel {nameof(TransactionRequest)} by key.")]
    [Trait("Category", "Unit")]
    public void CanCancelRequest()
    {
        // Arrange
        var requests = new TransactionRequest[]
        {
            CreateStubRequest(5),
            CreateStubRequest(4),
        };

        var transactionsRequestCache = new TransactionsRequestCache();
        transactionsRequestCache.AddRange(requests);

        // Act
        transactionsRequestCache.CancelRequest(new InternalID(5));
        var cancelledRequest = transactionsRequestCache.GetByKey(new InternalID(5))!;

        // Assert
        requests.Select(x => transactionsRequestCache.Contains(x)).Should().AllBeEquivalentTo(true);
        cancelledRequest.IsCancelled.Should().BeTrue();
    }

    [Fact(DisplayName = $"The {nameof(TransactionsRequestCache)} can provide thread safe add.")]
    [Trait("Category", "Unit")]
    public async void CanProvideThreadSafeAddAsync()
    {
        // Arrange
        var provider1Links = Enumerable.Range(1, 100)
            .Select(x => CreateStubRequest(x));
        var provider2Links = Enumerable.Range(101, 100)
            .Select(x => CreateStubRequest(x));

        var transactionsRequestCache = new TransactionsRequestCache();
        var mres = new ManualResetEventSlim();

        var t1 = Task.Run(() =>
        {
            mres.Wait();
            transactionsRequestCache.AddRange(provider1Links);
        });
        var t2 = Task.Run(() =>
        {
            mres.Wait();
            transactionsRequestCache.AddRange(provider2Links);
        });

        // Act
        mres.Set();
        await Task.WhenAll(t1,t2);

        // Assert
        provider1Links.Concat(provider2Links).Select(x => transactionsRequestCache.Contains(x)).Should().AllBeEquivalentTo(true);
    }

    private static TransactionRequest CreateStubRequest(long id)
    {
        var fromAccount = new BankAccount("01234012340123401234");
        var toAccount = new BankAccount("01234012340123401234");
        var transferredBalance = 121.4m;
        var transactions = new List<Transaction>()
        {
            new Transaction(
                fromAccount,
                toAccount,
                transferredBalance)
        };

        return new TransactionRequest(new InternalID(id), transactions);
    }
}
