using System;
using System.Reflection;

namespace AC.AvalonControlsLibrary.Core
{
    /// <summary>
    /// Helper class for reflection
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Returns a Proeprty info for a specific property
        /// </summary>
        /// <param name="obj">The object to get the property from</param>
        /// <param name="propertyName">The property name</param>
        /// <returns>Returns the property info of the type</returns>
        public static PropertyInfo GetPropertyForObject(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName);
        }
    
        /// <summary>
        /// InterfacePresentInClass check that an inteface is implimented for the specific dll type
        /// </summary>
        /// <param name="checkType">The type to check i.e the class</param>
        /// <param name="compareType">The interface type</param>
        /// <returns>Returns true if the interface is implemented in the class</returns>
        public static bool InterfacePresentInType(Type checkType, Type compareType)
        {
            TypeFilter interfaceFilter = InterfaceFilter;
            Type[] list = checkType.FindInterfaces(interfaceFilter, compareType);
            if (list != null && list.Length > 0)
                return true;

            return false;
        }

        /// <summary>
        /// InterfaceFilter will check if the type passed is the same as the object passed
        /// </summary>
        /// <param name="typeObj">The Type object to which the filter is applied</param>
        /// <param name="criteriaObj">An arbitrary object used to filter the list</param>
        /// <returns>Returns true to include the Type in the filtered list</returns>
        private static bool InterfaceFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
                return true;
            else
                return false;
        }
    }
}
