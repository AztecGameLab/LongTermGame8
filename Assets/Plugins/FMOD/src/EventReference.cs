using System;

namespace Plugins.FMOD.src
{
    [Serializable]
    // NOTE: Structs are custom data values/structures that can be used to store & organize related data
    public struct EventReference
    {
        // NOTE: global:: goes to the global namespace rather than the namespace written above
        // NOTE: GUID is a Globally Unique Identifier used to identify scenes, prefabs, etc
        public global::FMOD.GUID guid;

                // NOTE: #if UNITY_EDITOR ensures that the code only runs in the editor, not in the build
        #if UNITY_EDITOR
                public string path;

                // NOTE: Func<input, output>
                // This Function takes a string in as an input, and outputs a GUID
                public static Func<string, global::FMOD.GUID> GuidLookupDelegate;

                
            
                // Returns the guid & path
                public override string ToString()
                {
                    return $"{guid} ({path})";
                     // Former Code --> string.Format("{0} ({1})", guid, path);
                }

                // Checks if both the path and guid are null
                public bool IsNull => string.IsNullOrEmpty(path) && guid.IsNull;
                
                /* Former Code 
                 * public bool IsNull
                   {
                       get
                       {
                            return string.IsNullOrEmpty(path) && guid.IsNull;
                       }
                   }
                 */

                // Returns the EventReference associated with the path
                public static EventReference Find(string path)
                {
                    // I guess the Func<> is null if Event Manager has yet to be initialized
                    if (GuidLookupDelegate == null)
                    {
                        // Throw exception
                        throw new InvalidOperationException("EventReference.Find called before EventManager was initialized");
                    }
                    // Return a new EventReference with the given path and guid
                    return new EventReference { path = path, guid = GuidLookupDelegate(path) };
                }
        #else 
                public override string ToString()
                {
                    return Guid.ToString();
                }

                public bool IsNull
                {
                    get
                    {
                        return Guid.IsNull;
                    }
                }
        #endif
    }
}
