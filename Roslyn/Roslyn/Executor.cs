using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using Microsoft.CodeAnalysis.Emit;
using System.Collections.Generic;
using System.Reflection;

namespace CSharpConsole
{
    class Executor
    {
        public static void Main()
        {
            
            SyntaxTree tree = CSharpSyntaxTree.ParseText(@"using System.Web.Http;
public class TestController : ApiController
{ 
	public string Get() { 
		return ""Hello World"";
	} 
 
	public string Get(int id) { 
		return ""Hello World ""+id;
	} 
}");
            //var tree = SyntaxTree.ParseText(text);


            //    var compiledCode = Compilation.Create(
            //"controllers.dll",
            //options: new CompilationOptions(outputKind: OutputKind.DynamicallyLinkedLibrary),
            //syntaxTrees: new[] { tree },
            //references: new[] { new MetadataFileReference(typeof(object).Assembly.Location), new MetadataFileReference(@"C:Program Files (x86)Microsoft ASP.NETASP.NET MVC 4AssembliesSystem.Web.Http.dll") });

            //        Assembly assembly;
            //        using (var stream = new MemoryStream())
            //        {
            //            EmitResult compileResult = compiledCode.Emit(stream);
            //            assembly = Assembly.Load(stream.GetBuffer());
            //        }

            //CSharpCompilation compilation = CSharpCompilation.Create(
            //    "RoslynExample",
            //    new[] { tree },
            //    new[] { MetadataReference.CreateFromFile(typeof(object).Assembly.Location) },
            //    new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            string path = Path.Combine(@"C:\Code Samples\Roslyn\packages\Microsoft.AspNet.WebApi.Core.5.2.3\lib\net45\System.Web.Http.dll");
            string webhttp = Path.GetFullPath(path);
            string assemblyName = "RoslynExample";
            MetadataReference[] references = new MetadataReference[]
            {
    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
    MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
    //MetadataReference.CreateFromFile(@"C:Program Files (x86)Microsoft ASP.NETASP.NET MVC 4AssembliesSystem.Web.Http.dll")
    MetadataReference.CreateFromFile(webhttp)
            };

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { tree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));


            //using (var dllStream = new MemoryStream())
            //{
            //    var emitResult = compilation.Emit(dllStream);
            //    if (!emitResult.Success)
            //    {
            //        // emitResult.Diagnostics
            //        Console.WriteLine("Failed Emit");
            //        Console.ReadKey();
            //    }
            //}

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

            var address = "http://localhost/Codegen";
            var conf = new HttpSelfHostConfiguration(new Uri(address));
            conf.Routes.MapHttpRoute(name: "DefaultApi",
            routeTemplate: "api/{controller}/{id}",
            defaults: new
            {
                id = RouteParameter.Optional
            }
             );

            var server = new HttpSelfHostServer(conf);
            server.OpenAsync().Wait();
            Console.ReadKey();

        }


    }
}
