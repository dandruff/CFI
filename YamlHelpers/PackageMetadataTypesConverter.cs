using CFI.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace CFI.YamlHelpers;

/// <summary>
/// Converts an <see cref="ExternalRepo"/> object from YAML. This is required because the ExternalRepo can be a mapped object or just a scalar (the url)
/// </summary>
public class PackageMetadataTypesConverter : IYamlTypeConverter
{
    public static readonly IYamlTypeConverter Instance = new PackageMetadataTypesConverter();

    bool IYamlTypeConverter.Accepts(Type type)
        => type switch
                {
                    Type t when t == typeof(ExternalRepo) => true,
                    Type t when t == typeof(ManualChangelog) => true,
                    _ => false
                };

    object? IYamlTypeConverter.ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        => type switch
                {
                    Type t when t == typeof(ExternalRepo) => ParseExternalRepo(parser),
                    Type t when t == typeof(ManualChangelog) => ParseManualChangelog(parser),
                    _ => throw new NotSupportedException($"Unknown type: {type}")
                };

    void IYamlTypeConverter.WriteYaml(IEmitter emitter, object? value, Type type, ObjectSerializer serializer)
        => throw new NotImplementedException();

    private static ExternalRepo ParseExternalRepo(IParser parser)
    {
        string? url = null;
        string? tag = null;
        string? commit = null;

        // Check to see if this is an object
        if (parser.TryConsume<MappingStart>(out _))
        {
            while(parser.TryConsume<Scalar>(out var scalar))
            {
                switch (scalar.Value)
                {
                    case "url":
                        url = parser.Consume<Scalar>().Value;
                        break;
                    case "tag":
                        tag = parser.Consume<Scalar>().Value;
                        break;
                    case "commit":
                        commit = parser.Consume<Scalar>().Value;
                        break;
                    default:
                        Console.WriteLine($"Ignoring unknown key: {scalar.Value}");
                        break;
                }
            }
            parser.Consume<MappingEnd>();
        }
        else
        {
            // Its not an object, get the simple scalar value as the url
            url = parser.TryConsume<Scalar>(out var scalar) ? scalar.Value : throw new Exception("Expected scalar");
        }

        return new ExternalRepo
        {
            Url = url!,
            Tag = tag,
            Commit = commit
        };
    }

    private static ManualChangelog ParseManualChangelog(IParser parser)
    {
        string? filename = null;
        string? markupType = null;

        // Check to see if this is an object
        if (parser.TryConsume<MappingStart>(out _))
        {
            while(parser.TryConsume<Scalar>(out var scalar))
            {
                switch (scalar.Value)
                {
                    case "filename":
                        filename = parser.Consume<Scalar>().Value;
                        break;
                    case "markup-type":
                        markupType = parser.Consume<Scalar>().Value;
                        break;
                    default:
                        Console.WriteLine($"Ignoring unknown key: {scalar.Value}");
                        break;
                }
            }
            parser.Consume<MappingEnd>();
        }
        else
        {
            // Its not an object, get the simple scalar value as the filename
            filename = parser.TryConsume<Scalar>(out var scalar) ? scalar.Value : throw new Exception("Expected scalar");
        }

        return new ManualChangelog
        {
            Filename = filename!,
            MarkupType = markupType
        };
    }
}
