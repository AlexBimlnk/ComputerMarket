using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using General.Logic.Commands;

namespace WalletTransaction.Logic.Commands;


public sealed class CreateTransactionRequestCommand : ICommand
{
    //private readonly

    public Task<ICommandResult> ExecuteAsync() => throw new NotImplementedException();
}
