using Unity.VisualScripting;
using UnityEngine;

public abstract class UIPanelBase : MonoBehaviour
{
    //vibration manager yazarsak buraya degisken olarak ekle

    public abstract void Init(); //vibration managerinin instanceini ata
    public abstract void DeInit();

    public virtual void OnButtonClick()
    {
        //vibration oynat
    }
}
