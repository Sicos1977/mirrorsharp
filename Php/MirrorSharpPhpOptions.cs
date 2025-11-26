using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Pchp.Core;
using Pchp.Library;
using Peachpie.Library.Scripting;
using Peachpie.Library.XmlDom;

namespace MirrorSharp.Php;

/// <summary>MirrorSharp options for PHP</summary>
public class MirrorSharpPhpOptions {
    /// <summary>Contains the list of assembly reference paths to be used, not configurable.</summary>
    public static readonly ImmutableArray<string> AssemblyReferencePaths = GatherPeachpieReferences();

    /// <summary>Whether to compile in Debug mode.</summary>
    public bool? Debug { get; set; }

    internal MirrorSharpPhpOptions() {
    }

    private static ImmutableArray<string> GatherPeachpieReferences() {
        var refKnownTypes = new[] {
            typeof(object), // mscorlib
            typeof(Context), // Peachpie.Runtime
            typeof(Strings), // Peachpie.Library
            typeof(XmlDom), // Peachpie.Library.XmlDom
            typeof(ScriptingProvider) // Peachpie.Library.Scripting
        };

        var list = refKnownTypes.Select(type => type.GetTypeInfo().Assembly).Distinct().ToList();
        var set = new HashSet<Assembly>(list);

        for (var i = 0; i < list.Count; i++) {
            var assembly = list[i];
            var refs = assembly.GetReferencedAssemblies();
            foreach (var refname in refs) {
                var refassembly = Assembly.Load(refname);
                if (refassembly != null && set.Add(refassembly)) list.Add(refassembly);
            }
        }

        return list.Select(ass => ass.Location).ToImmutableArray();
    }
}