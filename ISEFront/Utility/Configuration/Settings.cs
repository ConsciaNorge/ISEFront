using ISEFront.api.ViewModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Web;

namespace ISEFront.Utility.Configuration
{
    public class Settings
    {
        private static SemaphoreSlim _lock = new SemaphoreSlim(1, 1);
        private static SettingsViewModel _settings = null;

        // Currently this property makes use of deep copy methods to update the settings because I don't 
        // want to risk threading related issues. I will consider writing a non-mutable alternative getter if
        // there is a performance issue.
        public static ISEServerSettingsViewModel IseServer {
            get
            {
                lockSettings();
                try
                {
                    loadIfNeeded();
                    var result = DeepCopyByExpressionTrees.DeepCopyByExpressionTree(_settings.IseServerSettings);
                    return result;
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Error loading settings" + e.Message);
                }
                finally
                {
                    unlockSettings();
                }
                return null;
            }
            set
            {
                lockSettings();
                try
                {
                    loadIfNeeded();
                    _settings.IseServerSettings = DeepCopyByExpressionTrees.DeepCopyByExpressionTree(value);
                    saveChanges();
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Error loading settings" + e.Message);
                }
                finally
                {
                    unlockSettings();
                }
            }
        }

        public static BankIDSettingsViewModel BankID
        {
            get
            {
                lockSettings();
                try
                {
                    loadIfNeeded();
                    var result = DeepCopyByExpressionTrees.DeepCopyByExpressionTree(_settings.BankIDSettings);
                    return result;
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Error loading settings" + e.Message);
                }
                finally
                {
                    unlockSettings();
                }
                return null;
            }
            set
            {
                lockSettings();
                try
                {
                    loadIfNeeded();
                    _settings.BankIDSettings = DeepCopyByExpressionTrees.DeepCopyByExpressionTree(value);
                    saveChanges();
                }
                catch (Exception e)
                {
                    Trace.WriteLine("Error loading settings" + e.Message);
                }
                finally
                {
                    unlockSettings();
                }
            }
        }

        private static string ConfigurationPath
        {
            get
            {
                return HttpContext.Current.Server.MapPath("~/app_data/isefront.config");
            }
        }

        private static void lockSettings(int timeOutMs=500)
        {
            if(!_lock.Wait(timeOutMs))
                throw new ThreadStateException("Failed to lock settings after 500ms");
        }

        private static void unlockSettings()
        {
            _lock.Release();
        }

        private static void loadIfNeeded()
        {
            try
            {
                var text = File.ReadAllText(ConfigurationPath);
                _settings = JsonConvert.DeserializeObject<SettingsViewModel>(text);
            }
            catch(Exception e)
            {
                Trace.WriteLine(e.Message);
                _settings = new SettingsViewModel();
            }
        }

        private static void saveChanges()
        {
            var text = JsonConvert.SerializeObject(_settings);
            File.WriteAllText(ConfigurationPath, text);
        }
    }
}
