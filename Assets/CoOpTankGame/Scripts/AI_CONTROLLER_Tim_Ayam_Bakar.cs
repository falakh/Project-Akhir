using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class AI_CONTROLLER_Tim_Ayam_Bakar : MonoBehaviour
{
    private Tank tank;

    private float distanceTressHolde = 2;
    //Kelas AI Controller adalah tempat anda memberikan kecerdasan buatan pada agen TANK anda
    //Peraturan yang harus dipenuhi adalah sbb :

    // Dilarang mengakses atau mengedit game objek tank enemy secara langsung menggunakan code unity apapun seperti :
    // GameObject.Find ,  GameObject.FindGameObjectsWithTag dan semacamnya, pelanggaran adalah diskualifikasi, nilai = 0, harap tanyakan terlebh dahulu terkait penggunaan fungsi2 tertentu;
    // seluruh script dilarang diedit, hanya script ini (AI_CONTROLLER) saja yang wajib anda ubah, karena script ini saja yg anda kumpulkan, error berarti gagal


    //Mengakses informasi enemmy telah di sediakan di script Tank.cs sbb
    //public Vector3 _infEnemyLastdirection;         //The last direction enemy facing, update when AI sensor found enemy.
    //public Vector3 _infEnemyLastPos;           //The last position of enemy, update when sensor found enemy
    //public int _infEnemyHealth;               //the last health of enemy.  update each frame
    //public int _infisEnemySeen;               //is enemy seen in sensor now.

    //contoh cara akses informasi di atas melalui kelas AI_CONTROLLER ini :
    //Vector3 a = GetComponent<Tank>()._infEnemyLastPos;
    //seluruh atribut kelas Tank.cs yang berawalan "_inf.." contoh _infEnemyHealth, boleh diakses
    //info mengenai atribut2 yang berawalan "_inf.." tsb silahkan buka pada kelas Tank.cs

    //Sensor OBSTACLE WALL dan MUSUH
    // tank anda telah dibekali sensor otomatis untuk mendeteksi wall dan musuh
    // sensor wall dengan  radius lingkaran 6 unit, sensor musuh segitiga sama kaki di depan tank 1.5 unit, Tinggi 12 unit dan alas = 10 unit
    // informasi dari sensor wall adalah posisi center tiap wall yg terdeteksi sensor, wall memiliki sisi selebar 1 unit bentuk persegi
    // informasi dari sensor wall dapat diakses di "GetComponent<Tank>()._infLastposWall" , berupa list<vector3> posisi center wall tersebut
    // informasi dari sensor musuh dapat diakses di "_infEnemyLastdirection, _infEnemyLastPos, _infEnemyHealth" diakses dengan getcomponent juga 

    // note untuk testing silahkan sesuaikan nilai attribut id pada kelas Tank.cs, id = 0 untuk player 1 dan id = 1 untuk player 2
    // silahkan konsulkan dengan dosen apa yg telah anda kerjakan agar tdk terkena diskualifikasi
    // silahkan melakukan read pada objek transform atau rigidbody tank anda sendiri namun tidak diperkenankan mengubah nilainya secara langsung
    // harus melalui methode yg di sediakan di kelas Tank.cs : "Move, Turn, Shoot"

    //MENGGERAKAN TANK, TANK MOVEMENT
    // Gerak MAJU dan MUNDUR
    //GetComponent<Tank>().Move(1); untuk maju
    //GetComponent<Tank>().Move(-1) ; untuk mundur

    // Gerak MEMUTAR atau merotasi arah depan tank
    //GetComponent<Tank>().Turn(-1); untuk putar ke kiri berlawanan jarum jam
    //GetComponent<Tank>().Turn(1); untuk putar ke kanan searah jarum jam

    // Menembak //SHOOTING
    //GetComponent<Tank>().Shoot()
    //peluru hanya 1, dan reload setiap 2 detik secara otomatis

    //telah disediakan framework behavior tree jika ingin menggunakan, silahkan bebas menggunakan rumus apapun, menggunakan vector dsb, algoritma apapun, yg tidak boleh adalah mengubah nilai langsung dari environment atau stats musuh atau stats anda sendiri menggunakan fungsi yg tdk dperkenankan
    //silahkan mengambil deltatime menggunakan Time.deltaTime, tdk masalah
    // kecurangan berakibat nilai = 0;


    // Use this for initialization

    private HashSet<Vector3> wallSheet;
    private HashSet<Vector3> visited;

    void Start()
    {
        visited = new HashSet<Vector3>();
        wallSheet = new HashSet<Vector3>();
        tank = GetComponent<Tank>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Tank>().rig.velocity =
            Vector2.zero; //disarankan jgn dihapus agar tank anda tidak jalan terus krn velocity, silahkan dihapus jika anda mengerti apa yg anda lakukan 
        visited.Add(transform.position);

        if (!tank._infIsEnemySeen || cekWall().Contains(WallStaus.Front))
        {
            Patrol();
        }
        else
        {
            var enemyPos = tank._infEnemyLastPos;
            var enemyVector = enemyPos - transform.position;
            var rotate = Vector3.Cross(enemyVector, transform.up);
            tank.Turn((int) rotate.normalized.z);

            if (rotate.normalized.z <= 1 && rotate.normalized.z >= 0)
            {
                tank.Shoot();
            }
        }
    }


    void Patrol()
    {
        if (cekWall().Length == 0)
        {
//            if (!cekWall().Contains(WallStaus.Front))
//            {
//                tank.Move(1);
//            }
//
//           else if (!cekWall().Contains(WallStaus.Right))
//            {
//                tank.Turn(1);
//            }
//
//           else if (!cekWall().Contains(WallStaus.Left))
//            {
//                tank.Turn(-1);
//            }
//
//            else if (!cekWall().Contains(WallStaus.Back))
//            {
//                tank.Turn(1);
//              
//            }

            tank.Move(1);
            visited.Add(transform.position);
        }
        else
        {
            tank.Turn(1);
        }
    }


    WallStaus[] cekWall()
    {
        HashSet<WallStaus> wallStatus = new HashSet<WallStaus>();
        foreach (var wall in tank._infLastposWall)
        {
            var temp = wall - transform.position;
            if (Vector3.Distance(wall, transform.position) < distanceTressHolde)
            {
                var heading = wall - transform.position;
                wallSheet.Add(wall);
                var dotHeading = Vector3.Dot(heading, transform.up);
                var dotLeft = Vector3.Dot(heading, -transform.right);
                var dotRight = Vector3.Dot(heading, transform.right);
                var dotBack = Vector3.Dot(heading, -transform.up);
                if (dotHeading >= 1)
                {
                    wallStatus.Add(WallStaus.Front);
                }

//                if (dotLeft >= 1)
//                {
//                    wallStatus.Add(WallStaus.Left);
//                }
//
//                if (dotRight >= 1)
//                {
//                    wallStatus.Add(WallStaus.Right);
//                }
//
//                if (dotBack >= 1)
//                {
//                    wallStatus.Add(WallStaus.Back);
//                }
            }
        }

        return wallStatus.ToArray();
    }


    float RecomRotation()
    {
        HashSet<WallStaus> wallStatus = new HashSet<WallStaus>();
        foreach (var wall in tank._infLastposWall)
        {
            var temp = wall - transform.position;
            if (Vector3.Distance(wall, transform.position) < distanceTressHolde)
            {
                var heading = wall - transform.position;
                wallSheet.Add(wall);
                var dotHeading = Vector3.Cross(heading, transform.up);
                return -dotHeading.normalized.z;
            }
        }

        return 0;
    }
}

enum WallStaus
{
    Front,
    Back,
    Left,
    Right
}