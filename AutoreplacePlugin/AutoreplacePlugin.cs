using RGiesecke.DllExport;
using System;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;

namespace Autoreplace
{

    public delegate IntPtr IdeGetPersonalPrefSets();

    public delegate IntPtr IdeGetPrefAsString(int pluginId, string prefSet, string name, string defaultValue);

    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool IdeGetPrefAsBool(int pluginId, string prefSet, string name, [MarshalAs(UnmanagedType.Bool)] bool defaultValue);

    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool IdeExecuteTemplate(string templateName, [MarshalAs(UnmanagedType.Bool)] bool newWindow);

    public delegate IntPtr IdeGetText();

    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool IdeSetText(string text);

    public delegate int IdeGetCursorX();

    public delegate int IdeGetCursorY();

    public delegate void IdeSetCursor(int column, int row);

    [return: MarshalAs(UnmanagedType.Bool)]
    public delegate bool IdeGetReadOnly();

    public delegate IntPtr IdeGetGeneralPref(string name);

    /// <summary>
    /// PL/SQL Developer plug-in to unwrap wrapped PL/SQL program units.
    /// </summary>
    public class AutoreplacePlugin
    {
        private static int pluginId;
        private const string PLUGIN_NAME = "Autoreplace plugin";
        private const int AUTOREPLACE_MENU_INDEX = 1;
        private const string TRUE = "True";

        private static IdeGetPersonalPrefSets getPrefSetsCallback;
        private static IdeGetPrefAsString getPrefCallback;
        private static IdeGetPrefAsBool getBoolPrefCallback;

        private static IdeExecuteTemplate execTemplateCallback;
        private static IdeGetText getTextCallback;
        private static IdeSetText setTextCallback;
        private static IdeGetCursorX getCursorXCallback;
        private static IdeGetCursorY getCursorYCallback;
        private static IdeSetCursor setCursorCallback;
        private static IdeGetReadOnly getReadOnlyCallback;
        private static IdeGetGeneralPref getGeneralPrefCallback;

        private const int IDE_GET_PREF_AS_STRING = 212;
        private const int IDE_GET_PREF_AS_BOOL = 214;
        private const int IDE_GET_PERSONAL_PREF_SETS = 210;
        private const int IDE_EXECUTE_TEMPLATE = 96;
        private const int IDE_GET_TEXT = 30;
        private const int IDE_SET_TEXT = 34;
        private const int IDE_GET_CURSOR_X = 141;
        private const int IDE_GET_CURSOR_Y = 142;
        private const int IDE_SET_CURSOR = 143;
        private const int IDE_GET_READ_ONLY = 26;
        private const int IDE_GET_GENERAL_PREF = 218;

        private static AutoreplacePlugin me;
        private AutoreplaceList autoReplaces;
        private DateTime cachedLastModifyDate;

        private AutoreplacePlugin()
        {
        }

        /// <summary>
        /// Unique plug-in ID assiged by PL/SQL Developer.
        /// </summary>
        public int Id
        {
            private set
            {
                pluginId = value;
            }
            get
            {
                return pluginId;
            }
        }

        public static AutoreplacePlugin Instance
        {
            get
            {
                if (me == null)
                {
                    me = new AutoreplacePlugin();
                }
                return me;
            }
        }

        /// <summary>
        /// Provides PL/SQL Developer with plug-in name.
        /// </summary>
        /// <param name="id">Plugin ID assigned by PL/SQL Developer.</param>
        /// <returns></returns>
        [DllExport("IdentifyPlugIn", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            Instance.Id = id;
            return PLUGIN_NAME;
        }

        /// <summary>
        /// This method is called by PL/SQL Developer to pass pointers to its API functions (callback functions).
        /// </summary>
        /// <param name="index">Index of a callback. See PL/SQL Developer plugins documentation.</param>
        /// <param name="function">Pointer to a function.</param>
        [DllExport("RegisterCallback", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            switch (index)
            {
                case IDE_GET_PREF_AS_STRING:
                    getPrefCallback = (IdeGetPrefAsString)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetPrefAsString));
                    break;
                case IDE_GET_PERSONAL_PREF_SETS:
                    getPrefSetsCallback = (IdeGetPersonalPrefSets)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetPersonalPrefSets));
                    break;
                case IDE_GET_PREF_AS_BOOL:
                    getBoolPrefCallback = (IdeGetPrefAsBool)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetPrefAsBool));
                    break;
                case IDE_EXECUTE_TEMPLATE:
                    execTemplateCallback = (IdeExecuteTemplate)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeExecuteTemplate));
                    break;
                case IDE_GET_TEXT:
                    getTextCallback = (IdeGetText)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetText));
                    break;
                case IDE_SET_TEXT:
                    setTextCallback = (IdeSetText)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeSetText));
                    break;
                case IDE_GET_CURSOR_X:
                    getCursorXCallback = (IdeGetCursorX)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetCursorX));
                    break;
                case IDE_GET_CURSOR_Y:
                    getCursorYCallback = (IdeGetCursorY)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetCursorY));
                    break;
                case IDE_SET_CURSOR:
                    setCursorCallback = (IdeSetCursor)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeSetCursor));
                    break;
                case IDE_GET_READ_ONLY:
                    getReadOnlyCallback = (IdeGetReadOnly)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetReadOnly));
                    break;
                case IDE_GET_GENERAL_PREF:
                    getGeneralPrefCallback = (IdeGetGeneralPref)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeGetGeneralPref));
                    break;
            }
        }

        /// <summary>
        /// Handles About button click in PL/SQL Developer's plug-ins list.
        /// </summary>
        /// <returns>Text to display in default about dialog.</returns>
        [DllExport("About", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static string About()
        {
            return @"Autoreplace plug-in like Ctrl+J in IntelliJ IDEA.";
        }

        [DllExport("CreateMenuItem", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            if (index == AUTOREPLACE_MENU_INDEX)
            {
                return "Tools / Autoreplace";
            }
            else
            {
                return "";
            }
        }

        public void ExecuteTemplate(string templateNameOrPath)
        {
            bool success = execTemplateCallback(ExtractTemplateName(templateNameOrPath), false);
            if (!success)
            {
                ShowError("Error executing template: " + templateNameOrPath);
            }
        }

        public void InsertText(string text)
        {
            string currentText = Marshal.PtrToStringAnsi(getTextCallback());
            int cursorColumn = getCursorXCallback();
            int cursorRow = getCursorYCallback();

            int zeroBasedOffset = CalculateZeroBasedOffset(currentText, cursorColumn, cursorRow);

            bool success = setTextCallback(currentText.Insert(zeroBasedOffset, text));
            if (!success)
            {
                ShowError("Cannot insert text.");
            }
            setCursorCallback(cursorColumn + text.Length, cursorRow);
        }

        /// <summary>
        /// Calculates zero-based offset from PL/SQL Developer row and column index.
        /// Is slow since calculates position of newline character.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="column"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        private int CalculateZeroBasedOffset(string text, int column, int row)
        {
            int zeroBasedOffset = -1;
            for (int i = 1; i < row; i++)
            {
                // When CR is new line character, the does not work.
                zeroBasedOffset = text.IndexOf('\n', zeroBasedOffset + 1);
            }
            return zeroBasedOffset + column;
        }

        private string ExtractTemplateName(string templateNameOrPath)
        {
            int i = templateNameOrPath.LastIndexOf(".tpl", StringComparison.OrdinalIgnoreCase);
            if (i >= 0)
            {
                return templateNameOrPath.Substring(0, i);
            }
            else
            {
                return templateNameOrPath;
            }
        }

        /// <summary>
        /// Displays error message box for plugin.
        /// </summary>
        /// <param name="message">Message text.</param>
        public static void ShowError(string message)
        {
            MessageBox.Show(message, PLUGIN_NAME, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// OnMenuClick event handler.
        /// </summary>
        /// <param name="index">Index of a clicked menu.</param>
        [DllExport("OnMenuClick", CallingConvention = System.Runtime.InteropServices.CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            bool autoreplaceClicked = (index == AUTOREPLACE_MENU_INDEX);

            if (!getReadOnlyCallback() && me.AutoreplaceEnabled)
            {
                me.ShowAutoreplaceList();
            }
        }

        private bool AutoreplaceFileChanged()
        {
            return true;
        }

        private string AutoreplaceFilePath
        {
            get
            {
                return Marshal.PtrToStringAnsi(getGeneralPrefCallback("AutoReplaceFilename"));
            }

        }

        private bool AutoreplaceEnabled
        {
            get
            {
                string enabled = Marshal.PtrToStringAnsi(getGeneralPrefCallback("AutoReplaceEnabled"));
                return (TRUE.Equals(enabled, StringComparison.InvariantCultureIgnoreCase));
            }

        }

        private void ShowAutoreplaceList()
        {
            if (autoReplaces == null)
            {
                autoReplaces = AutoreplaceList.LoadFromFile(AutoreplaceFilePath);
                cachedLastModifyDate = File.GetLastWriteTime(AutoreplaceFilePath);
            }
            else
            {
                DateTime lastModifyDate = File.GetLastWriteTime(AutoreplaceFilePath);
                if (DateTime.Compare(lastModifyDate, cachedLastModifyDate) > 0)
                {
                    cachedLastModifyDate = lastModifyDate;
                    autoReplaces = AutoreplaceList.LoadFromFile(AutoreplaceFilePath);
                }
            }
            AutoreplaceForm form = new AutoreplaceForm();
            form.Show(autoReplaces, me);
        }

        public void DoReplace(AutoreplaceEntry entry)
        {
            bool isTemplate = entry.Value.EndsWith(".tpl", StringComparison.OrdinalIgnoreCase);
            if (isTemplate)
            {
                me.ExecuteTemplate(entry.Value);
            }
            else
            {
                me.InsertText(entry.Value);
            }
        }
    }
}

