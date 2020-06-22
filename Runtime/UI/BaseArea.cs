using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pedroleonardo.UI
{

	public abstract class BaseArea : MonoBehaviour
	{

		public event UINavigation.OnNavigationEventDelegate OnNavigationEven;

		[SerializeField]
		private string areaName;
		public string AreaName
		{

			get
			{
				return areaName;
			}

		}

		public virtual void Init(ArrayList param)
		{

		}

		public virtual void Open()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.startOpen, this);

			OnOpenEnded();

		}

		protected virtual void OnOpenEnded()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.endOpen, this);
		}

		public virtual void Close()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.startClose, this);

			OnCloseEnded();
		}

		protected virtual void OnCloseEnded()
		{

			if (OnNavigationEven != null)
				OnNavigationEven(UINavigation.AreaState.endClose, this);
		}

	}
}