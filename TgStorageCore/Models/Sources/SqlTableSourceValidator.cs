﻿// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com

using FluentValidation;

namespace TgStorageCore.Models.Sources;

[DebuggerDisplay("{nameof(TableSourceValidator)}")]
public class SqlTableSourceValidator : AbstractValidator<SqlTableSourceModel>
{
    #region Public and private fields, properties, constructor

    public SqlTableSourceValidator()
    {
        RuleFor(item => item.Id)
                .NotEmpty()
                .NotNull()
                .NotEqual(0);
        //RuleFor(item => item.UserName)
        //        .NotEmpty()
        //        .NotNull();
    }

    #endregion
}