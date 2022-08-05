using System;
using Microsoft.CodeAnalysis.CSharp;

namespace CommunityToolkit.Mvvm.SourceGenerators.ComponentModel.Models;

sealed record PropertyAccess
{
    /// <summary>
    /// Access modifier for the whole property.
    /// </summary>
    public SyntaxKind Property { get; }
    
    /// <summary>
    /// Optional access modifier for the getter.
    /// </summary>
    public SyntaxKind? Getter { get; }
    
    /// <summary>
    /// Optional access modifier for the setter.
    /// </summary>
    public SyntaxKind? Setter { get; }

    private PropertyAccess(SyntaxKind property, SyntaxKind? getter, SyntaxKind? setter)
    {
        Property = property;
        Getter = getter;
        Setter = setter;
    }

    public static PropertyAccess Default { get; } = new (SyntaxKind.PublicKeyword, null, null);

    public static PropertyAccess FromEnumValues(int? getter, int? setter)
    {
        if (getter == setter)
        {
            return new PropertyAccess(GetSyntaxKindFromEnumValue(getter), null, null);
        }
        else if (getter == null || getter < setter)
        {
            return new PropertyAccess(GetSyntaxKindFromEnumValue(getter), null, GetSyntaxKindFromEnumValue(setter));
        }
        else
        {
            return new PropertyAccess(GetSyntaxKindFromEnumValue(setter), GetSyntaxKindFromEnumValue(getter), null);
        }
    }

    private static SyntaxKind GetSyntaxKindFromEnumValue(int? value)
    {
        return value switch
        {
            null => SyntaxKind.PublicKeyword,
            1 => SyntaxKind.PublicKeyword,
            2 => SyntaxKind.ProtectedKeyword,
            3 => SyntaxKind.InternalKeyword,
            4 => SyntaxKind.PrivateKeyword,
            _ => throw new ArgumentOutOfRangeException(nameof(value), "Invalid access modifier value: " + value)
        };
    }
}