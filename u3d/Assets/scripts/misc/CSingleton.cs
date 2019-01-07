

//Sigleton
public class CSingleton<T>
	where T : new()
{
	private static T m_sInstance;
	public static T instance
	{
		get
		{
			if(m_sInstance == null)
			{
				m_sInstance = new T();
			}
			return m_sInstance;
		}
	}

	//destroy all memory of data
	public virtual void Reset()
	{
		m_sInstance = default(T);
	}
}