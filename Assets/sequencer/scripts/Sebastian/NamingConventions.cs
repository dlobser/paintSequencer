using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace HolojamEngine {
    /// Pascal Case - Capitalized the first letter for each word in the context e.g. HelloWorld. 
    /// Camel Case - Capitalized the first letter for each word in the context except the first word e.g. helloWorld. 

    /// Use uppercase notation when you have to deal with acronyms e.g. IP, UI. 
    /// Acronyms that deals with more than three letters must follow the .Net framework to be consistent and use PascalCase e.g. Xml instead of XML. 

    /// <summary> 
    /// Naming Rules: PascalCase 
    /// </summary> 
    /// <remarks> 
    /// DO: 
    ///     * Use CompanyName.TechnologyName[.Feature][.Design] 
    ///     * Use plural name for namespaces "Collections" over "Collection",  
    ///         exception to the rule is whenever you deal with acronyms e.g. "IO" over "IOs". 
    ///     * Use a logical hierarchical order,  
    ///         classes that are deeper in the tree are most likely dependent on classes that found within their parent namespace. 
    /// DO NOT: 
    ///     * Use the same name for namespaces whereas you have classes already definded under the same name. 
    /// </remarks> 
    namespace Template {
        /// <summary> 
        /// Naming Rules: PascalCase 
        /// </summary> 
        /// <remarks> 
        /// DO: 
        ///     * Prefix interfaces with the letter "I". 
        ///     * Name interfaces with nouns or noun phrases or adjectives describing behavior e.g IComponent, IEnumberable  
        /// </remarks> 
        public interface IInterface {
        }

        /// <summary> 
        /// Naming Rules: Noun, PascalCase 
        /// </summary> 
        /// <remarks> 
        /// DO: 
        ///     * Use abbreviations sparingly. 
        ///     * Use plural suffix whenever the enumeration representing a set of bitwise flags, else use a singular name for most enum types. 
        ///     * Add the "FlagsAttribute' whenever the enumeration is a bitwise enum type. 
        /// DO NOT: 
        ///     * Prefix (or suffix) enums or its values with "Enum" nor use the enum name in conjunction with its values. 
        /// </remarks> 
        public enum Enum : int {
            /// <summary> 
            /// Naming Rules: Verb | Noun, PascalCase 
            /// </summary> 
            ItemOne,
            ItemTwo,
        }

        /// <summary> 
        /// Naming Rules: Noun, PascalCase 
        /// </summary> 
        /// <remarks> 
        /// DO: 
        ///     * Add the suffix EventHandler when it suppose to represent an event. 
        ///     * Use two parameters for events: 
        ///         object sender - the caller that raised the event. 
        ///         EventArgs e - the event argument class. 
        /// DO NOT: 
        ///     * Use the "EventHandler" if you aren't going to represent an event delegation. 
        /// </remarks> 
        /// <param name="arg">Noun, camelCase</param> 
        public delegate void DelegateNameEventHandler(object sender, EventArgs e);
        public delegate void UIObjectUpdate(object arg);

        /// <summary> 
        /// Naming Rules: Noun, PascalCase 
        /// </summary> 
        /// <remarks> 
        /// DO: 
        ///     * Choose short names for base / abstract classes,  
        ///         they should be literally short and use abbreviations sparingly (try to use nouns instead abbreviations). 
        /// </remarks> 
        public class BaseClass {
        }

        /// <summary> 
        /// Naming Rules: Noun, PascalCase 
        /// </summary> 
        /// <remarks> 
        /// DO: 
        ///     * Prefix the drived class with the base class name (if possible). 
        ///     * Suffix any class that represents exception with "Exception". 
        ///     * Suffix any class that represents attribute with "Attribute". 
        ///     * Suffix any class that represents event argument with "EventArgs". 
        /// </remarks> 
        public class NamingConventions {
            #region P/Invoke 
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            internal static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);
            #endregion
            /// Keyword Rules: [accessibility level][modifier][name]
            /// <summary>
            /// Naming Rules: Noun, PascalCase
            /// </summary>
            //const byte[] IP = { 127, 0, 0, 1 };
            const string LocalIP = "127.0.0.1";
            /// <summary>
            /// Naming Rules: Noun, PascalCase
            /// Access Modifier: Public
            /// </summary>
            public int X, Y;
            /// <summary>
            /// Naming Rules: Noun PascalCase
            /// Access Modifier: Protected
            /// </summary>
            protected int A, B;
            /// <summary>
            /// Naming Rules: Noun, camelCase
            /// Access Modifier: Private
            /// </summary>
            private int x, y;
            /// <summary>
            /// Naming Rules: Noun, camelCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Use plural name to describe arrays.
            /// </remarks>
            private string[] names;
            /// <summary>
            /// Naming Rules: Noun, camelCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Use plural name to describe a closed-type list e.g. "items" over "itemList".
            /// NOTE:
            ///     * As a general rule, use the generic type-parameter in its plural form (if possible).
            /// </remarks>
            private IList<object> items;
            /// <summary>
            /// Naming Rules: Noun, camelCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Use plural name to describe a collection e.g. "items" over "itemCollection".
            /// NOTE:
            ///     * As a general rule, use the type the collection represents (if possible).
            /// </remarks>
            private ItemCollection itemsz;
            /// <summary>
            /// Naming Rules: Noun, camelCase
            /// Keyword Modifier: Static
            /// Access Modifier: Private
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Prefix private statics with an underscore.
            /// </remarks>
            static private int _x, _y;
            /// <summary>
            ///  Naming Rules: Verb, camelCase
            /// </summary>
            /// <remarks>
            /// NOTE:
            ///     * Delegates instances representing "type of an action".
            /// </remarks>
            public UIObjectUpdate uiObjectUpdater;
            /// <summary>
            ///  Naming Rules: Verv, PascalCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Use the delegate's name without its suffix for events.
            ///     * Give straightforward names for activities e.g. Click, DoubleClick
            ///     * Prefix events with "Pre" for present and "Post" for past whenever the timing entry for the object state to change cannot be determined.
            ///         - The only thing the user know here is that the state is about to occur or alrady occurred e.g. PreLoad.
            ///     * Use the verb suffix, "ing" for current and "ed" for past whenever the timing entry of the state to change is known.
            ///         - The user can predict when will the object state will change e.g. SelectedIndexChanged.
            /// NOTE:
            ///     * Events use PascalCase regardless to delegate instances.
            /// </remarks>
            public event DelegateNameEventHandler DelegateName;
            #region Constructor(s)
            public NamingConventions() {
                // ...
            }
            public NamingConventions(object temp)
                : this() {
                // ...
            }
            #endregion

            #region Instantiation
            /// <summary>
            /// Naming Rules: Verb, PascalCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Prefix methods that create objects with "Create".
            /// </remarks>
            public static object Create() {
                return new object();
            }
            #endregion
            #region Initialization
            /// <summary>
            /// Naming Rules: Verb, PascalCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Name methods that only have to initialize an object as "Initialize"..
            /// </remarks>
            public static void Initialize() {
            }
            #endregion
            #region Indexer(s)
            // ...
            #endregion
            /// Apply the same rules set for all properties.
            #region Group Of Properties
            /// <summary>
            /// Naming Rules: Noun | Noun Phrase, PascalCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Try to avoid abbreviations.
            ///     * Consider to define a property name based on its field's name.
            /// </remarks>
            public string Property {
                get { return null; }
                set { }
            }
            #endregion

            /// Apply the same rules set for all methods.
            #region Group Of Methods
            /// <summary>
            /// Naming Rules: Verb, PascalCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Try to avoid abbreviations.
            /// NOTE:
            ///     * Inside the methods body you can go with whatever convention you feel is appropriate.
            /// </remarks>
            /// <param name="arg">Noun, camelCase</param>
            public void Function(object obj) {
            }
            #endregion
            #region Method(s) Implementation
            // ...
            #endregion
            #region Abstract Method(s)
            // ...
            #endregion
            #region Private Methods(s)
            // ...
            #endregion
            #region Destructor(s)
            // ...
            #endregion
            #region ItemCollection
            /// <summary>
            /// Naming Rules: Noun, PascalCase
            /// </summary>
            /// <remarks>
            /// DO:
            ///     * Tag any collection prefix with the type it's representing and "Collection" as its suffix. 
            /// </remarks> 
            public class ItemCollection {
            }
            #endregion
        }

        /// <summary> 
        /// Naming Rules: Noun, PascalCase 
        /// </summary> 
        public struct Struct {
        }
    }
}

