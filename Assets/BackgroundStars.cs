using UnityEngine;

public class BackgroundStars : MonoBehaviour
{
    public GameObject backgroundChunkPrefab;
    public GameObject starPrefab;
    private GameObject[][] chunks = new GameObject[3][];
    private float chunkSize;
    public float starCount = 10f;
    private float starCountAdjusted
    {
        get { return (starCount * chunkSize); }
    }
    private GameObject centralChunk {
        get { return chunks[1][1]; }
    }
    private GameObject playerShip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    void Awake()
    {
        InitializeChunks();
        ArrangeChunks();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPosition = playerShip.transform.position;
        Vector3 centralChunkPosition = centralChunk.transform.position;
        float halfChunk = chunkSize / 2f;
        if (playerPosition.x > centralChunkPosition.x + halfChunk) ArrangeChunks("shift-right");
        else if (playerPosition.x < centralChunkPosition.x - halfChunk) ArrangeChunks("shift-left");
        if (playerPosition.y > centralChunkPosition.y + halfChunk) ArrangeChunks("shift-top");
        else if (playerPosition.y < centralChunkPosition.y - halfChunk) ArrangeChunks("shift-bottom");
    }

    // Initializes the chunks, populates them with stars and moves them to player position
    void InitializeChunks()
    {
        // Initialize chunk array
        for (int i = 0; i < 3; i++)
        {
            chunks[i] = new GameObject[3];
            for (int j = 0; j < 3; j++)
            {
                chunks[i][j] = Instantiate<GameObject>(backgroundChunkPrefab);
            }
        }
        // Initialize chunk size
        Collider chunkCollider = centralChunk.GetComponent<Collider>();
        chunkSize = chunkCollider.bounds.size.y;
        // Move central chunk to player
        playerShip = GameObject.FindWithTag("Player");
        centralChunk.transform.position = playerShip.transform.position;
        // Populate chunks with stars
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                PopulateChunkWithStars(chunks[i][j]);
            }
        }
    }

    // Populates the chunk with star sprites
    void PopulateChunkWithStars(GameObject chunk)
    {
        float halfChunk = chunkSize / 2f;
        for (int i = 0; i < starCountAdjusted; i++)
        {
            GameObject star = CreateRandomizedStar(chunk);
            star.transform.localPosition = new Vector3(
                Random.Range(-halfChunk, halfChunk),
                Random.Range(-halfChunk, halfChunk),
                0f);
        }
    }
    
    // Creates a star with randomized size, transparency and rotation
    GameObject CreateRandomizedStar(GameObject parent)
    {
        GameObject star = Instantiate<GameObject>(starPrefab, parent.transform);
        // Random transparency
        SpriteRenderer sr = star.GetComponent<SpriteRenderer>();
        Color color = sr.color;
        color.a = Random.Range(0f, 0.2f);
        sr.color = color;
        // Random rotation
        star.transform.localRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        // Random size
        float scaleAmount = Random.Range(0.5f, 1f);
        star.transform.localScale = new Vector3(scaleAmount, scaleAmount, 1);
        return star;
    }

    // Shifts and arranges chunks based on chunks array with respect to the middle element
    void ArrangeChunks(string shiftChunks = null)
    {
        // Shift chunks
        if (shiftChunks == "shift-left")
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject last = chunks[i][2];
                chunks[i][2] = chunks[i][1];
                chunks[i][1] = chunks[i][0];
                chunks[i][0] = last;
            }
        } else if (shiftChunks == "shift-right")
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject last = chunks[i][2];
                chunks[i][2] = chunks[i][0];
                chunks[i][0] = chunks[i][1];
                chunks[i][1] = last;
            }
        } else if (shiftChunks == "shift-bottom")
        {
            GameObject[] last = chunks[2];
            chunks[2] = chunks[1];
            chunks[1] = chunks[0];
            chunks[0] = last;
        }
        else if (shiftChunks == "shift-top")
        {
            GameObject[] last = chunks[2];
            chunks[2] = chunks[0];
            chunks[0] = chunks[1];
            chunks[1] = last;
        }

        // Place 8 outer chunks around the center chunk
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (i == 1 && j == 1) continue;
                Vector3 newPosition = centralChunk.transform.position;
                if (i < 1) newPosition.y -= chunkSize;
                else if (i > 1) newPosition.y += chunkSize;
                if (j < 1) newPosition.x -= chunkSize;
                else if (j > 1) newPosition.x += chunkSize;
                chunks[i][j].transform.position = newPosition;
            }
        }
    }
}
