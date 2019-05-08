using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tank : MonoBehaviour 
{

    //rule
    // info internal dan external yg boleh diambil, 
    //untuk info internal lain boleh di read namun tdk boleh di update langsung (contoh : transform , rigidbodi)
    // untuk pergerakan gunakan methode publik yg sdh dsediakan  di kelas tank "move", "shoot", "turn", tdk dperkenankan mengubah transform secara langsung
    [Header("daftar Stats / attribut yg boleh diambil,untuk id sesuaikan player (0 u/ plyer 1, 1 u/ plyer 2")]
    [Tooltip("id sesuaikan player (0 u/ plyer 1, 1 u/ plyer 2")]
    public int id;                          //The unique identifier for this player.
    public int _infhealth;                      //The current health of the tank.
    public int _infmaxHealth;                   //The maximum health of this tank.
    public int _infdamage;                      //How much damage this tank can do when shooting a projectile.
    public float _infmoveSpeed;                 //How fast the tank can move.
    public float _infturnSpeed;                 //How fast the tank can turn.
    public float _infprojectileSpeed;           //How fast the tank's projectiles can move.
    public float _infreloadSpeed;              //How many seconds it takes to reload the tank, so that it can shoot again.
    public float _infreloadTimer;				//A timer counting up and resets after shooting.
    public Vector3 _infvelocity;				//A timer counting up and resets after shooting.
    public List <Vector3> _infLastposWall = new List<Vector3>() ;
    public LayerMask wallLayer;
    [HideInInspector]
	public Vector3 _infdirection;               //The direction that the tank is facing. Used for movement direction.
    public Vector3 _infEnemyLastdirection;               //The last direction enemy facing.
    public Vector3 _infEnemyLastPos;               //The last position of enemy.
    public int _infEnemyHealth;               //the last health of enemy.
    public bool _infIsEnemySeen;               //is enemy seen in sensor now.
    //// batas Stats / attribut yg boleh diambil


    //private stats, can't change except gameManager
    //Gk boleh diambil atau otak atik

    private int health;                      //The current health of the tank.
    private int maxHealth;                   //The maximum health of this tank.
    private int damage;                      //How much damage this tank can do when shooting a projectile.
    private float moveSpeed;                 //How fast the tank can move.
    private float turnSpeed;                 //How fast the tank can turn.
    private float projectileSpeed;           //How fast the tank's projectiles can move.
    private float reloadSpeed;              //How many seconds it takes to reload the tank, so that it can shoot again.
    private float reloadTimer;              //A timer counting up and resets after shooting.

    private float _radiusSensorWall = 6;

    //[Header("Bools / gk boleh diedit")]
    [HideInInspector]
    public bool canMove;                    //Can the tank move if it wants to?
    [HideInInspector]
    public bool canShoot;					//Can the tank shoot if it wants to?

	[Header("Components / Objects, gk boleh diedit")]
	public Rigidbody2D rig;					//The tank's Rigidbody2D component. 
	public GameObject projectile;			//The projectile prefab of which the tank can shoot.
	public GameObject deathParticleEffect;	//The particle effect prefab that plays when the tank dies.
	public Transform muzzle;				//The muzzle of the tank. This is where the projectile will spawn.
	private Game game;                       //The Game.cs script, located on the GameManager game object.

    //  batas yg gak boleh diambil


    void Awake()
    {
        game = GameObject.Find("GameManager").GetComponent<Game>();
        _infEnemyLastPos = new Vector3(-1000,-1000,-1000);
    }

    void Start ()
	{
		_infdirection = Vector3.up;	//Sets the tank's direction up, as that is the default rotation of the sprite.
        SetStartValues();
	}

	//Called by the Game.cs script when the game starts.
	private void SetStartValues ()
	{
		//Sets the tank's stats based on the Game.cs start values.
		health = game.tankStartHealth;
		maxHealth = game.tankStartHealth;
		damage = game.tankStartDamage;
		moveSpeed = game.tankStartMoveSpeed;
		turnSpeed = game.tankStartTurnSpeed;
		projectileSpeed = game.tankStartProjectileSpeed;
		reloadSpeed = game.tankStartReloadSpeed;

        // setupInfo
        _infhealth = health;
        _infmaxHealth = maxHealth;
        _infdamage = damage;
        _infmoveSpeed = moveSpeed;
        _infturnSpeed = turnSpeed;
        _infprojectileSpeed = projectileSpeed;
        _infreloadSpeed = reloadSpeed;
        _infreloadTimer = reloadTimer;
        

    }

	void Update ()
	{

        _infvelocity = rig.velocity;
        reloadTimer += Time.deltaTime;
        _infreloadTimer = reloadTimer;
        cekWall();
        
    }

    

    //Called by the Controls.cs script. When a player presses their movement keys, it calls this function
    //sending over a "y" value, set to either 1 or 0, depending if they are moving forward or backwards.
    public void Move (int y)
	{
        if (y > 1)
        {
            y = 1;
        }
        else if (y < -1)
        {
            y = -1;
        }
        rig.velocity = _infdirection * y * moveSpeed * Time.deltaTime;
       // cekWall();
    }

	//Called by the Controls.cs script. When a player presses their turn keys, it calls this function
	//sending over an "x" value, set to either 1 or 0, depending if they are moving left or right.
	public void Turn (int x)
	{
        if (x > 1)
        {
            x = 1;
        }
        else if (x < -1)
        {
            x = -1;
        }
        transform.Rotate(-Vector3.forward * x * turnSpeed * Time.deltaTime);
		_infdirection = transform.rotation * Vector3.up;
	}

	//Called by the Contols.cs script. When a player presses their shoot key, it calls this function, making the tank shoot.
	public void Shoot ()
	{
		if(reloadTimer >= reloadSpeed){													//Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
			GameObject proj = Instantiate(projectile, muzzle.transform.position, Quaternion.identity) as GameObject;	//Spawns the projectile at the muzzle.
			Projectile projScript = proj.GetComponent<Projectile>();					//Gets the Projectile.cs component of the projectile object.
			projScript.tankId = id;														//Sets the projectile's tankId, so that it knows which tank it was shot by.
			projScript.damage = damage;													//Sets the projectile's damage.
			projScript.game = game;														

			projScript.rig.velocity = _infdirection * projectileSpeed * Time.deltaTime;		//Makes the projectile move in the same direction that the tank is facing.
			reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
            //
            _infreloadTimer = reloadTimer;
        }
	}

	//Called when the tank gets hit by a projectile. It sends over a "dmg" value, which is how much health the tank will lose. 
	public void Damage (int dmg)
	{
		if(game.oneHitKill){	//Is the game set to one hit kill?
			Die();				//If so instantly kill the tank.
			return;
		}

		if(health - dmg <= 0){	//If the tank's health will go under 0 when it gets damaged.
			Die();				//Kill the tank since its health will be under 0.
		}else{					//Otherwise...
			health -= dmg;		//Subtract the dmg from the tank's health.
            _infhealth = health;
		}
	}

	//Called when the tank's health is or under 0.
	public void Die ()
	{
		if(id == 0){				//If the tank is player 1.
			game.player2Score++;	//Add 1 to player 2's score.
		}
		if(id == 1){				//If the tank is player 2.
			game.player1Score++;	//Add 1 to player 1's score.
		}

		canMove = false;			//The tank can now not move.
		canShoot = false;			//The tank can now not shoot.

		//Particle Effect
		GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity) as GameObject;	//Spawn the death particle effect at the tank's position.
		Destroy(deathEffect, 1.5f);						//Destroy that effect in 1.5 seconds.

		transform.position = new Vector3(0, 100, 0);	//Set the tanks position outside of the map, so that it is not visible when dead.

		StartCoroutine(RespawnTimer());					//Start the RespawnTimer coroutine.
	}

	//Called when the tank has been dead and is ready to rejoin the game.
	private void Respawn ()
	{
		canMove = true;
		canShoot = true;

		health = maxHealth;
        //
        _infhealth = health;

		transform.position = game.spawnPoints[Random.Range(0, game.spawnPoints.Count)].transform.position;	//Sets the tank's position to a random spawn point.
	}

	//Called when the tank dies, and needs to wait a certain time before respawning.
	IEnumerator RespawnTimer ()
	{
		yield return new WaitForSeconds(game.respawnDelay);	//Waits how ever long was set in the Game.cs script.

		Respawn();											//Respawns the tank.
	}


    void cekWall()
    {
        _infLastposWall.Clear();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), _radiusSensorWall,wallLayer);

        foreach (Collider2D wall in hitColliders)
        {
            _infLastposWall.Add(wall.gameObject.transform.position);
          //  Debug.Log(wall.gameObject.transform.position);
        }
       // Debug.Log(_infLastposWall.Count);
    }
}
