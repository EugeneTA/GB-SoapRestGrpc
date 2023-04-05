using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PumpService
{
    public class ScriptService : IScriptService
    {
        private CompilerResults _compilerResults;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _settingsService;
        private readonly IPumpServiceCallback _callback;

        public ScriptService(
            IStatisticsService statisticsService,
            ISettingsService settingsService,
            IPumpServiceCallback callback)
        {
            _statisticsService = statisticsService;
            _settingsService = settingsService;
            _callback = callback;
        }

        public bool Compile()
        {
            try
            {
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                // Подключаем и текущую сборку
                compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                FileStream fileStream = new FileStream(_settingsService.FileName, FileMode.Open);

                byte[] buffer;

                try
                {
                    int lenght = (int)fileStream.Length;
                    buffer = new byte[lenght];
                    int bytesRead;
                    int sum = 0;
                    while ((bytesRead = fileStream.Read(buffer, sum, lenght - sum)) > 0) sum += bytesRead;
                }
                finally
                {
                    fileStream.Close();
                }

                CSharpCodeProvider provider = new CSharpCodeProvider();
                _compilerResults = provider.CompileAssemblyFromSource(compilerParameters, Encoding.UTF8.GetString(buffer));

                if (_compilerResults.Errors != null && _compilerResults.Errors.Count != 0)
                {
                    string compilerErrors = string.Empty;

                    foreach (var error in _compilerResults.Errors)
                    {
                        if (string.IsNullOrEmpty(compilerErrors)) { compilerErrors += "\n"; }

                        compilerErrors += error.ToString();
                    }

                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public void Run(int count)
        {
            if (_compilerResults == null || (_compilerResults != null && _compilerResults.Errors != null && _compilerResults.Errors.Count > 0))
            {
                if (Compile() == false) { return; }
            }

            Type type = _compilerResults.CompiledAssembly.GetType("Sample.SampleScript");
            
            if (type == null) { return; }

            MethodInfo entryPointMethod = type.GetMethod("EntryPoint");

            if (entryPointMethod == null) { return; }


            Task.Run(() =>
            {
                for (int i = 0; i < count; i++)
                {
                    if ((bool)entryPointMethod.Invoke(Activator.CreateInstance(type), null))
                    {
                        _statisticsService.SucceesTacts++;
                    }
                    else
                    {
                        _statisticsService.ErrorTacts++;
                    }
                    _callback.UpdateStatistics((StatisticsService)_statisticsService);
                    Thread.Sleep(1000);
                }
            });



            
        }
    }
}