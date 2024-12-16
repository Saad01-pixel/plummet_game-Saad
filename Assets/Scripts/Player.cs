using UnityEngine;
using System.Collections.Generic;
using MongoDB.Driver;
using MongoDB.Bson;

public class Player : MonoBehaviour
{

    public string plummie_tag;
    public int collisions;
    public int steps;

    // Nom de la base de données MongoDB et de la collection où nous stockerons les scores
    private string connectionString = "your_mongodb_connection_string"; // Remplacez par connection
    private string databaseName = "GameProgression";
    private string collectionName = "progression";

    public string Stringify()
    {
        return JsonUtility.ToJson(this);
    }

    public static PlayerData Parse(string json)
    {
        return JsonUtility.FromJson<PlayerData>(json);
    }

    // Sauve les donner du joueur
    public async Task SavePlayerDataAsync()
    {
        // Creer un objet JSON 
        var scoreData = new BsonDocument
        {
            { "game", "NomPrenomEtudiant_fall_guy" },
            { "score", this.collisions },
            { "timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
        };

        // Connexion Mongo
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);

        // Le score dans la base de donnee
        try
        {
            await collection.InsertOneAsync(scoreData);
            Debug.Log("Score sauvegardé avec succès !");
        }
        catch (Exception )
        {
            Debug.LogError($"Erreur de sauvegarde dans MongoDB : ");
        }


    // Vitesse de mouvement du joueur
    public float speed = 1.5f;
    private Rigidbody2D rigidBody2D;
    private Vector2 movement;
    private PlayerData playerData;
    private bool _isGameOver;
    
    // Question 6 : Ajout d'un mode AI  - attribut pour activer le mode AI

    private List<Vector2> path; //chemin de Djikstra
    private int currentPathIndex; 
    private float pathRecalculationTime = 1f; //frequence de recalcul du chemin
    private float lastPathCalculationTime; //duree de calcul du chemin
    private float stuckCheckTime = 3.5f; //frequence de detection de chemin bloque
    private float lastStuckCheckTime; //derniere fois ou la detection de chemin bloque a ete faite
    private Vector2 lastPosition; //derniere position du joueur
    private int stuckCounter = 0; //nombre de fois ou le chemin est bloque
    private float backtrackDistance = 2f; // distance de recul pour essayer un nouveau chemin

    //Question 4 : Gestion de l'évènement fin du jeu
    public static event System.Action OnGameOver;

    private ScoreManager scoreManager; // referance au scoremanager


    void Start()
    {
        _isGameOver = false;
        rigidBody2D = GetComponent<Rigidbody2D>();
        playerData = new PlayerData();
        playerData.plummie_tag = "nraboy";
        path = new List<Vector2>();
        lastPosition = transform.position;
        // Question 6 : Ajout d'un mode AI  - calcul du chemin
        // rajouter l'appel de fonction requis

        //Question 4 : Gestion de l'évènement fin du jeu
        // score manager referance
        scoreManager = FindObjectOfType<ScoreManager>();

        // Evenement gameover
        OnGameOver += RestartGame;
    }


    void CheckGameOver()
    {
        // 
        if (rigidBody2D.position.x > 24.0f && !_isGameOver)
        {
            _isGameOver = true;
            Debug.Log("Reached the finish line!");
            OnGameOver?.Invoke(); // Déclenche l'événement OnGameOver
        }

       
    }

    // Méthode qui sera appelée lors de la fin du jeu
    void RestartGame()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Redemarre la scene
    }

    void Update()
    {
        // Verification si il atteint la ligne
        if (!_isGameOver)
        {
            // Logic movement

            CheckGameOver(); // Vérifie si le jeu est terminé (ligne d'arrivée ou énergie)
        }
    }

    void FixedUpdate()
    {
        if (!_isGameOver)
        {
            Vector2 newPosition = rigidBody2D.position + movement * Time.fixedDeltaTime;
            rigidBody2D.MovePosition(newPosition);
        }
    }

    // Méthode qui s'execute lorsque il y a collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        playerData.collisions++;

        // Le code pour vérifier les collisions et gérer le mode AI peut être ajouté ici
    }

    // Dessin et coloration des gizmos
    void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }

    //methode qui calcule le chemin a prendre en Mode AI
    void CalculatePath()
    {
        if (Time.time - lastPathCalculationTime < pathRecalculationTime)
            return;

        lastPathCalculationTime = Time.time;
        Vector2 startPos = transform.position;
        
        // Add random vertical offset to try different paths
        float verticalOffset = Random.Range(-2f, 2f);
        Vector2 targetPos = new Vector2(25f, transform.position.y + verticalOffset);
        
        path = Dijkstra.FindPath(startPos, targetPos, stuckCounter);
        currentPathIndex = 0;
        stuckCounter = 0; // Reset stuck counter when finding new path
    }

    //methode pour verifier si le joueur est bloque en mode AI
    void CheckIfStuck()
    {
        if (Time.time - lastStuckCheckTime < stuckCheckTime)
            return;

        float distanceMoved = Vector2.Distance(transform.position, lastPosition);
        if (distanceMoved < 0.1f && !_isGameOver)
        {
            stuckCounter++;
            if (stuckCounter > 3) // If stuck for too long
            {
                BacktrackAndFindNewPath();
            }
        }
        else
        {
            stuckCounter = 0;
        }

        lastPosition = transform.position;
        lastStuckCheckTime = Time.time;
    }

    //retour en arriere et recherche d'un nouveau chemin
    void BacktrackAndFindNewPath()
    {
        // le mode AI fait reculer le joueur
        Vector2 backtrackPosition = transform.position;
        backtrackPosition.x -= backtrackDistance;
        transform.position = backtrackPosition;

        // le chemin actuel est efface et un nouveau chemin est recalcule
        path.Clear();
        currentPathIndex = 0;
        lastPathCalculationTime = 0f; // met a 0 le delai de temps pour le calcul du chemin
        CalculatePath();
    }

    void Update()
    {
        //Question 3: Calcul et mise à jour des scores 
        public async Task LoadPlayerDataAsync()
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            var collection = database.GetCollection<BsonDocument>(collectionName);

            // Rechercher les scores précédents (ici nous cherchons tous les documents)
            var filter = Builders<BsonDocument>.Filter.Eq("game", "NomPrenomEtudiant_fall_guy");
            var results = await collection.Find(filter).ToListAsync();

            if (results.Count > 0)
            {
                // Traiter les résultats
                Debug.Log("Scores précédents récupérés !");
            }
            else
            {
                Debug.Log("Aucun score trouvé.");
            }



            if (!_isGameOver)
        {
            //introduire le mode AI dans cette fonction
            //si le jeu est en mode AI alors le AI controlle le mouvement 
            //sinon le joueur peut controller le mouvement

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            movement = new Vector2(h * speed, v * speed);


                //Question 6 - Mode AI
                if (!isAIMode)
                {
                    // Si le mode AI est deactiver
                    
                    float h = Input.GetAxis("Horizontal");
                    float v = Input.GetAxis("Vertical");
                    movement = new Vector2(h * speed, v * speed);
                }
                else
                {
                    // Si le mode AI est activer
                    UpdateAIMovement();  // Met à jour le mouvement 
                    CheckIfStuck();      // Ver si le joueur est bloquer
                }
                //Question 5 : Sauvegarde de la progression du jeu 
            }

    //mouvement automatique controlle par le mode AI
    void UpdateAIMovement()
    {
        if (path == null || path.Count == 0 || currentPathIndex >= path.Count)
        {
            CalculatePath();
            return;
        }

        Vector2 currentTarget = path[currentPathIndex];
        Vector2 currentPosition = transform.position;
        Vector2 direction = (currentTarget - currentPosition);
        float distance = direction.magnitude;

        if (distance < 0.1f)
        {
            currentPathIndex++;
            if (currentPathIndex >= path.Count)
            {
                CalculatePath();
                return;
            }
        }

        movement = direction.normalized * speed;
    }
    
    //verification du progres du joueur
    void FixedUpdate()
    {
        if (!_isGameOver)
        {
            Vector2 newPosition = rigidBody2D.position + movement * Time.fixedDeltaTime;
            rigidBody2D.MovePosition(newPosition);
        }

        if (rigidBody2D.position.x > 24.0f && _isGameOver == false)
        {
            _isGameOver = true;
            Debug.Log("Reached the finish line!");
        }
    }

    //methode qui s'execute lorsque il y a collision
    void OnCollisionEnter2D(Collision2D collision)
    {
                playerData.collisions++;  // incremente

                // Question 6 : Ajout d'un mode AI - Vérifie si le joueur est bloqué après une collision
                if (isAIMode)  // Si le mode AI est act
                {
                    stuckCounter++; // Incremente

                    if (stuckCounter > 2) // joueur bloquer
                    {
                        // full
                        BacktrackAndFindNewPath();
                    }
                    else
                    {
                        // Sinon recalcule chemin
                        CalculatePath();
                    }
                }
            }

    //dessin et coloration des gizmos
    void OnDrawGizmos()
    {
        if (path != null && path.Count > 0)
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < path.Count - 1; i++)
            {
                Gizmos.DrawLine(path[i], path[i + 1]);
            }
        }
    }

    void OnEnable()  
    {
        ScoreManager.OnGameOver += RestartGame;
    }

    void OnDisable()
    {
        ScoreManager.OnGameOver -= RestartGame;
    }


}
