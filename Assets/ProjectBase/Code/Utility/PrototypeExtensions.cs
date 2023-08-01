using System;

namespace Utility
{
    public static class PrototypeExtensions
    {
        public static TOutProperty GetPropertyRecursive<TPrototype, TProperty, TOutProperty>(
            TPrototype prototype,
            Func<TPrototype, TPrototype> getParentFunc,
            Func<TPrototype, TProperty> getPropertyFunc,
            Func<TProperty, TOutProperty> getOutProperty
            )
        {
            TProperty property = default;
            TPrototype lookingPrototype = prototype;
            while (lookingPrototype != null)
            {
                property = getPropertyFunc(lookingPrototype);
                if (property != null)
                {
                    break;
                }
                lookingPrototype = getParentFunc(lookingPrototype);
            }
            return getOutProperty(property);
        }
    }
}