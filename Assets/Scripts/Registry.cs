using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DwarfMiningGame
{
    public static class Registry<T> where T : UnityEngine.Object, IIdentifiable
    {
        private static Dictionary<string, T> _obj;
        private static bool _isInitialized = false;

        private static void Init()
        {
            _isInitialized = true;
            _obj = new Dictionary<string, T>();

            T[] assets = Resources.LoadAll<T>( "" );

            foreach( var asset in assets )
            {
                _obj.Add( asset.ID, asset );
            }
        }

        private static void InitIfNotInitialized()
        {
            if( !_isInitialized )
            {
                Init();
            }
        }

        public static bool Exists( string id )
        {
            InitIfNotInitialized();

            return _obj.ContainsKey( id );
        }

        public static bool Exists( string id, out T obj )
        {
            InitIfNotInitialized();

            return _obj.TryGetValue( id, out obj );
        }

        public static T Get( string id )
        {
            InitIfNotInitialized();

            return _obj[id];
        }

        public static void RegisterObj( T obj )
        {
            InitIfNotInitialized();

            _obj[obj.ID] = obj;
        }
    }
}