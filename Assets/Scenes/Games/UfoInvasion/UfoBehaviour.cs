using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoBehaviour : MonoBehaviour
{
    public GameObject LaserPrefab, UfoReleasePrefab;
    public float speed;
    public enum UfoType
    {
        Beige,
        Blue,
        Green,
        Pink,
        Yellow
    }
    public UfoType Type;

    // Start is called before the first frame update
    void Start()
    {
        if (this.Type == UfoType.Pink) StartCoroutine(UseLasers());
        if (this.Type == UfoType.Yellow) StartCoroutine(UseUfoRelease());
    }

    IEnumerator UseUfoRelease()
    {
        yield return new WaitForSeconds(3);
        Instantiate(UfoReleasePrefab, this.transform);
        StartCoroutine(UseUfoRelease());
    }
    IEnumerator UseLasers()
    {
        yield return new WaitForSeconds(5);
        GameObject a = Instantiate(LaserPrefab, this.transform);
        GameObject b = Instantiate(LaserPrefab, this.transform);
        GameObject c = Instantiate(LaserPrefab, this.transform);
        GameObject d = Instantiate(LaserPrefab, this.transform);
        a.GetComponent<LaserUfoBehaviour>().Initialize(new Vector2(10, 10));
        b.GetComponent<LaserUfoBehaviour>().Initialize(new Vector2(-10, 10));
        c.GetComponent<LaserUfoBehaviour>().Initialize(new Vector2(10, -10));
        d.GetComponent<LaserUfoBehaviour>().Initialize(new Vector2(-10, -10));
        StartCoroutine(UseLasers());
    }

    // Update is called once per frame
    void Update()
    {
        if (this.Type == UfoType.Beige || this.Type == UfoType.Pink || this.Type == UfoType.Yellow)
            this.transform.position = new Vector3(this.transform.position.x - speed, this.transform.position.y, this.transform.position.z);
    }
}
