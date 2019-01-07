using UnityEngine;
using System.Collections;

//Sigleton with Mono code
public class CSingletonMono<T> : MonoBehaviour
	where T : MonoBehaviour
{
	private static T m_sInstance;
	public static T instance
	{
		get
		{
			if (m_sInstance == null)
			{
				m_sInstance = (T)FindObjectOfType (typeof(T));
 
				if (m_sInstance == null)
				{
					GameObject obj = new GameObject(typeof(T).Name);
					m_sInstance = obj.AddComponent<T>();
					DontDestroyOnLoad(obj);
				}
			}
 
			return m_sInstance;
		}
	}

	public virtual void Awake()
	{
		if(m_sInstance == null)
		{
			m_sInstance = this as T;
		}
	}

	public virtual void OnDestroy()
	{
		if( m_sInstance == this ){
			m_sInstance = default(T);
		}
	}

	public static bool IsValid()
	{
		return ( m_sInstance != null ) ;
	}
}