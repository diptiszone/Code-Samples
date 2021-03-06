﻿using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.CodeAnalysis.Text;

namespace CSharpConsole
{
    class Executor
    {
        public static void Main()
        {

            var controllerpath = @"C:\Code Samples\Roslyn\Roslyn\controllers.txt";
            using (var stream = File.OpenRead(controllerpath))
            {
                var tree = CSharpSyntaxTree.ParseText(SourceText.From(stream), path: controllerpath);

                string path = Path.Combine(@"C:\Code Samples\Roslyn\packages\Microsoft.AspNet.WebApi.Core.5.2.3\lib\net45\System.Web.Http.dll");
                string webhttp = Path.GetFullPath(path);

                string selfhostpath = Path.Combine(@"C:\Code Samples\Roslyn\packages\Microsoft.AspNet.WebApi.SelfHost.5.2.3\lib\net45\System.Web.Http.SelfHost.dll");
                string codeanalysispath = Path.Combine(@"C:\Code Samples\Roslyn\packages\Microsoft.CodeAnalysis.Common.2.3.2\lib\netstandard1.3\Microsoft.CodeAnalysis.dll");
                string codeanalysiscsharppath = Path.Combine(@"C:\Code Samples\Roslyn\packages\Microsoft.CodeAnalysis.CSharp.2.3.2\lib\netstandard1.3\Microsoft.CodeAnalysis.CSharp.dll");
                string systempath = Path.Combine(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.6.1\System.dll");
                string webnethttp = Path.Combine(@"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Net.Http.dll");

                string assemblyName = "RoslynExample";
                MetadataReference[] references = new MetadataReference[]
                {
    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
    MetadataReference.CreateFromFile(webhttp),
    MetadataReference.CreateFromFile(selfhostpath),
    MetadataReference.CreateFromFile(codeanalysispath),
    MetadataReference.CreateFromFile(codeanalysiscsharppath),
    MetadataReference.CreateFromFile(systempath),
    MetadataReference.CreateFromFile(webnethttp)
                };

                CSharpCompilation compilation = CSharpCompilation.Create(
                    assemblyName,
                    syntaxTrees: new[] { tree },
                    references: references,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


                using (var ms = new MemoryStream())
                {
                    EmitResult result = compilation.Emit(ms);

                    if (!result.Success)
                    {
                        IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                            diagnostic.IsWarningAsError ||
                            diagnostic.Severity == DiagnosticSeverity.Error);

                        foreach (Diagnostic diagnostic in failures)
                        {
                            Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                        }
                        Console.ReadKey();
                    }
                    else
                    {
                        ms.Seek(0, SeekOrigin.Begin);
                        Assembly assembly = Assembly.Load(ms.ToArray());
                    }
                }
               
            }

            //var address = "http://localhost/";
            //var conf = new HttpSelfHostConfiguration(new Uri(address));
            //conf.Routes.MapHttpRoute(name: "DefaultApi",
            //routeTemplate: "{servicename}/{controller}/{id}",
            //defaults: new
            //{
            //    //servicename = "Codegen",
            //    servicename = "Helloworld",
            //    id = RouteParameter.Optional
            //}
            // );

            //var server = new HttpSelfHostServer(conf);
            //server.OpenAsync().Wait();
            Console.ReadKey();

        }
        
    }
}
