﻿/**
 * Helper Class by thers
 */

using CitizenFX.Core;
using CitizenFX.Core.Native;
using System;

namespace SirenControls {
    public enum DecorationType {
        Float = 1,
        Bool = 2,
        Int = 3,
        Time = 5
    }

    public class EntityDecoration {
        protected static Type floatType = typeof(float);
        protected static Type boolType = typeof(bool);
        protected static Type intType = typeof(int);

        public static bool ExistOn(Entity entity, string propertyName) {
            return Function.Call<bool>(Hash.DECOR_EXIST_ON, entity.Handle, propertyName);
        }

        public static void RegisterProperty(string propertyName, DecorationType type) {
            Function.Call(Hash.DECOR_REGISTER, propertyName, (int)type);
        }

        public static void Set(Entity entity, string propertyName, float floatValue) {
            Function.Call(Hash._DECOR_SET_FLOAT, entity.Handle, propertyName, floatValue);
        }

        public static void Set(Entity entity, string propertyName, int intValue) {
            Function.Call(Hash.DECOR_SET_INT, entity.Handle, propertyName, intValue);
        }

        public static void Set(Entity entity, string propertyName, bool boolValue) {
            Function.Call(Hash.DECOR_SET_BOOL, entity.Handle, propertyName, boolValue);
        }

        public static T Get<T>(Entity entity, string propertyName) {
            if (!ExistOn(entity, propertyName)) {
                throw new EntityDecorationUnregisteredPropertyException();
            }

            Type genericType = typeof(T);
            Hash nativeMethod;

            if (genericType == floatType) {
                nativeMethod = Hash._DECOR_GET_FLOAT;
            } else if (genericType == intType) {
                nativeMethod = Hash.DECOR_GET_INT;
            } else if (genericType == boolType) {
                nativeMethod = Hash.DECOR_GET_BOOL;
            } else {
                throw new EntityDecorationUndefinedTypeException();
            }

            return (T)Function.Call<T>(nativeMethod, entity.Handle, propertyName);
        }
    }

    public class EntityDecorationUnregisteredPropertyException : Exception { }
    public class EntityDecorationUndefinedTypeException : Exception { }
}
