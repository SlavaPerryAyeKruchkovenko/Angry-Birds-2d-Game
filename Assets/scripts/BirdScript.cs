using Assets.scripts;
using Assets.scripts.Angry_Birds_2d_BusnesLogic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public GameObject FlyMaterial;
    private Bird _bird;
    private bool _canDrawPoint = true;
    private readonly int _steps = 50;
    private const float _g = 9.8f;
    private CancellationTokenSource _token = new CancellationTokenSource();

    private void Awake()
    {
        gameObject.GetComponent<LineRenderer>().positionCount = _steps;
        if (_bird == null)
        {
            _bird = gameObject.GetComponent<GameObjectScript>().ABGameObj as Bird;
            if (_bird != null && _canDrawPoint)
            {
                _bird.StartFly += DeleteFlyPoints;
            }
        }
    }
    public async void DrawPoints(CancellationTokenSource token)
    {
        for (int i = 0; i <= 100; i++)
        {
            if (token.Token.IsCancellationRequested)
            {
                return;
            }
            else if (gameObject.GetComponent<Rigidbody2D>())
            {
                Instantiate(FlyMaterial, gameObject.transform.position, default);
            }
            await Task.Delay(5);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_bird != null && _canDrawPoint && collision.gameObject.CompareTag("Slingshot"))
        {
            DrawPoints(_token);
            _canDrawPoint = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_bird is IBird birdAbility &&
            (birdAbility.Ability == TypeUsingAbility.TouchObject || birdAbility.Ability == TypeUsingAbility.Universal))
        {
            birdAbility.UsePower();
            _bird.GetDamage(1);
        }
    }
    private void DeleteFlyPoints()
    {
        foreach (var item in GameObject.FindGameObjectsWithTag("point"))
        {
            Destroy(item);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Background") && gameObject.GetComponent<Rigidbody2D>() &&
            _bird is IBird ibird && ibird.Ability == TypeUsingAbility.Click)
        {
            _token.Cancel();
            ibird.UsePower();
        }
    }
    public void DrawTraectory(Vector3 range)
    {
        var coordinate = CountPoints(gameObject.transform.position, range);
        gameObject.GetComponent<LineRenderer>().SetPositions(coordinate);
    }
    private Vector3[] CountPoints(Vector3 posicion, Vector3 impulse)
    {
        Vector3[] results = new Vector3[_steps];
        Vector2 newImpulse = new Vector2(impulse.x, impulse.y);
        float speed = (newImpulse / _bird.Mass).magnitude; // v = p/m
        float corner = gameObject.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        float time = 0;

        for (int i = 0; i < _steps; i++)
        {
            time += 0.1f;
            float x = speed * Mathf.Cos(corner) * time;// x = |v| * cos(a) * t
            float y = speed * Mathf.Sin(corner) * time - _g * Mathf.Pow(time, 2) / 2; //y = v0 * sina * t - g * t^2 / 2
            Vector3 point = new Vector2(x, y);
            results[i] = posicion + point;
        }
        return results;
    }
}
