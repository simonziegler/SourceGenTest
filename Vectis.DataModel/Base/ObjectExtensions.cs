using System.Collections.Generic;
using System.Reflection;
using System.ArrayExtensions;
using System.ComponentModel.DataAnnotations;

namespace System
{
    /// <summary>
    /// Sourced from https://github.com/Burtsev-Alexey/net-object-deep-copy
    /// Renamed the original "Copy" method to "DeepCopy" for clarity. Tests to perform a deep copy with Newtonsoft
    /// and MessagePack in June 2020 indicate that they are 10 and 3 times slower respectively.
    /// </summary>
    public static class ObjectExtensions
    {
        private static readonly MethodInfo CloneMethod = typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);


        /// <summary>
        /// Determines if the type is primitive, and includes "string" in that determination.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPrimitive(this Type type)
        {
            if (type == typeof(string)) {
                return true;
            }
            return type.IsValueType & type.IsPrimitive;
        }


        /// <summary>
        /// Performs a recursive deep copy of an object.
        /// </summary>
        /// <param name="source">The object to be copied.</param>
        /// <returns></returns>
        public static T DeepCopy<T>(this T source) where T : class
        {
            return InternalCopy(source, new Dictionary<object, object>(new ReferenceEqualityComparer())) as T;
        }


        private static object InternalCopy(object source, IDictionary<object, object> visited)
        {
            if (source == null)
            {
                return null;
            }

            var typeToReflect = source.GetType();

            if (IsPrimitive(typeToReflect))
            {
                return source;
            }
            
            if (visited.ContainsKey(source)) 
            {
                return visited[source];
            }

            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) 
            {
                return null;
            }
            
            var cloneObject = CloneMethod.Invoke(source, null);

            if (typeToReflect.IsArray)
            {
                var arrayType = typeToReflect.GetElementType();
                
                if (!IsPrimitive(arrayType))
                {
                    Array clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }

            visited.Add(source, cloneObject);
            
            CopyFields(source, visited, cloneObject, typeToReflect);
            
            RecursiveCopyBaseTypePrivateFields(source, visited, cloneObject, typeToReflect);
            
            return cloneObject;
        }


        private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
        {
            if (typeToReflect.BaseType != null)
            {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }


        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
        {
            foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
            {
                if ((filter != null && !filter(fieldInfo)) || IsPrimitive(fieldInfo.FieldType))
                {
                    continue;
                }

                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }


        /// <summary>
        /// Gets a display attribute for the given property name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="propertyName">The property name for which the display attribute is required.</param>
        /// <returns></returns>
        [Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static DisplayAttribute GetDisplayAttribute<T>(this T obj, string propertyName)
        {
            return typeof(T).GetProperty(propertyName).GetCustomAttribute<DisplayAttribute>();
        }
    }


    /// <summary>
    /// A class overriding <see cref="EqualityComparer{T}.Equals(T, T)"/> method with <see cref="object.ReferenceEquals(object, object)"/>.
    /// </summary>
    public class ReferenceEqualityComparer : EqualityComparer<object>
    {
        /// <summary>
        /// Overrides <see cref="EqualityComparer{T}.Equals(T, T)"/> method with <see cref="object.ReferenceEquals(object, object)"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }


        /// <summary>
        /// Override to allow a null obj parameter, returning zero for nulls.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int GetHashCode(object obj)
        {
            if (obj == null) 
            {
                return 0;
            }

            return obj.GetHashCode();
        }
    }

    namespace ArrayExtensions
    {
        public static class ArrayExtensions
        {
            public static void ForEach(this Array array, Action<Array, int[]> action)
            {
                if (array.LongLength == 0) 
                {
                    return;
                }

                ArrayTraverse walker = new ArrayTraverse(array);
                do action(array, walker.Position);
                while (walker.Step());
            }
        }


        internal class ArrayTraverse
        {
            public int[] Position { get; set; }
            private readonly int[] maxLengths;


            public ArrayTraverse(Array array)
            {
                maxLengths = new int[array.Rank];
                for (int i = 0; i < array.Rank; ++i)
                {
                    maxLengths[i] = array.GetLength(i) - 1;
                }
                Position = new int[array.Rank];
            }


            public bool Step()
            {
                for (int i = 0; i < Position.Length; ++i)
                {
                    if (Position[i] < maxLengths[i])
                    {
                        Position[i]++;
                        for (int j = 0; j < i; j++)
                        {
                            Position[j] = 0;
                        }
                        return true;
                    }
                }
                return false;
            }
        }
    }

}
