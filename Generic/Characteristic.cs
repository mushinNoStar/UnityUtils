private delegate void characteristicChangedMethod<T>(Characteristic<T> chr, T oldValue);
private delegate void OnChangeMethod<T>(CharacteristicModificationData<T> data);

/// <summary>
/// A characteristic should be used as a wrap around of a int or a string.
/// It allows the OnChanged and AfterChange events.
/// </summary>
/// <typeparam name="T">T should really be an immutabile type,
/// else it can't keep track of it's internal changes.</typeparam>
public class Characteristic<T>
{
    private T value;
    private bool locked = false;

    /// <summary>
    /// called before the change actually applies.
    /// </summary>
    public event characteristicChangedMethod<T> AfterChange;

    /// <summary>
    /// called after the change actually applies, do not use to set a new value.
    /// </summary>
    public event OnChangeMethod<T> OnChange;

	public Characteristic(T ogg)
	{
        value = ogg;
	}

    public T get()
    {
        return value;
    }

    public bool isLocked()
    {
        return locked;
    }

    /// <summary>
    /// calls OnChange, then i applies, then calls AfterChange.
    /// if the characteristic is already being changed, this will throw an access violation exception.
    /// </summary>
    public void set(T val)
    {
        if (locked)
            throw new System.AccessViolationException("The characteristic was locked, have you tried to use set() from a event reciver?");

        locked = true;

        CharacteristicModificationData<T> data = new CharacteristicModificationData<T>(this, val);
        if (OnChange != null)
        {
            OnChange(data);
            if (data.ignored) //ignoring the change has priority over other modifications.
            {
                locked = false;
                return;
            }
        }
        T oldValue = value;
        value = data.modifiableNewValue;

        if (AfterChange != null)
            AfterChange(this, oldValue);

        locked = false;
    }

    /// <summary>
    /// A characteristic should be behave as the T type.
    /// </summary>
    public static implicit operator T(Characteristic<T> characteristic)
    {
        return characteristic.value;
    }

    /// <summary>
    /// no reason to show this outside of this file.
    /// </summary>
    private class CharacteristicModificationData<T>
    {
        /// <summary>
        /// The characteristic that is being changed.
        /// </summary>
        public Characteristic<T> characteristic;
        /// <summary>
        /// The parameter that has been passed when char.set() was called.
        /// This cannot be changed and will be ignored by the set function.
        /// Notice that if T is not an immutabile type, this will be equal to modifiableNewValue.
        /// </summary>
        public readonly T newValue;
        /// <summary>
        /// The current modified value, this is the one that will be applied.
        /// </summary>
        public T modifiableNewValue;
        /// <summary>
        /// If this is set as true, the change will not be applied.
        /// </summary>
        public bool ignored = false;

        private CharacteristicModificationData(Characteristic<T> chr, T newVal)
        {
            characteristic = chr;
            modifiedNewValue = chr;
            newValue = newVal;
        }
    }
}
