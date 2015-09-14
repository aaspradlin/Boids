using UnityEngine;
using System.Collections;

//TODO somehow add in an option for steady-state selection

/** <summary>
 * This class is the template for a classic genetic algorithm. </summary> */
public class GeneticAlgorithm : Algorithm {

	/** <summary>
	 * The number of boids initially generated in a flock. </summary> */
	private const int NUM_BOIDS = 20;

	/** <summary>
	 * The number of members in each generation. </summary> */
	private readonly int MEMBERS = 30;
	
	/** <summary>
	 * The number of generations in a full genetic algorithm run. </summmary> */
	private readonly int GENERATIONS = 30;
	
	/** <summary>
	 * The number of current generation members (with the highest fitnesses to move on to the next 
	 * generation. </summary> */
	private readonly int ELITES = 3;
	
	/** <summary>
	 * The chance that a bit in a generated genome will change from mutation. </summary> */
	private readonly float MUTATION_RATE = 0.07f;

	/** <summary>
	 * The number of population members to generate randomly in every generation. </summary> */
	private readonly int RANDOM_MEMBERS = 1;
	
	/** <summary>
	 * <see cref="Select()"/> selects two parents using the Roulette Wheel selection strategy when passed this constant. 
	 * This involves choosing mates based on their fitness level. It's not good to use when the fitnesses of members 
	 * vary widely. </summary> */
	public static readonly int ROULETTE_SELECTION = 1;

	/** <summary>
	 * <see cref="Select()"/> selects two parents using the Rank selection strategy when passed this constant. 
	 * Selection is based on the fitness of each chromosome but is more even than roulette. </summary> */
	public static readonly int RANK_SELECTION = 2;
	
	/** <summary>
	 * The runtime (in seconds) for each member in the population. </summary> */
	private readonly float RUN_TIME = 10;
	
	/* ------------------------------------------------------------------------------------- */
	
	/** <summary>
	 * The current flock being run. </summary> */
	private GameObject flock;

	/** <summary>
	 * The member number of the current flock being run. </summary> */
	private int membersCounter = -1;

	/** <summary>
	 * The total fitness of a generation. </summary> */
	private float totalFitness = 0;
	
	/** <summary>
	 * The current generation's <see cref="GAGenome"/>s. </summary> */
	private GAGenome[] currentGeneration;
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Primary constructor. </summary>
	 * <param name="name"> The name of the algorithm </param>
	 * <param name="members"> The number of members in a generation </param>
	 * <param name="generations"> The number of getnerations in a run </param>
	 * <param name="elites"> The number of elites in breeding </param>
	 * <param name="random_members> The number of members to randomly generate for each population </param>
	 * <param name="mutation_rate"> The probability of mutation </param>
	 * <param name="run_time"> The runtime of each member in the population </param> */
	public GeneticAlgorithm(string name, int members, int generations, int elites, int random_members, float mutation_rate, float run_time) : base(name) {

		//these include checks for valid input. If anything is invalid, the default value is used.
		if (generations > 0) GENERATIONS = generations;
		if (elites >= 0) ELITES = elites;
		if (random_members >= 0) RANDOM_MEMBERS = random_members;
		if (members > 3 && members >= ELITES + RANDOM_MEMBERS) MEMBERS = members;
		if (mutation_rate >= 0 && mutation_rate < 1) MUTATION_RATE = mutation_rate;
		if (run_time > 2.5f) RUN_TIME = run_time;
	}
	
	/** <summary>
	 * Constructor that uses default values. </summary>
	 * <param name="name"> The name of the algorithm </param> */
	public GeneticAlgorithm(string name) : base(name) { }
	
	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary>
	 * Sets up the scene to begin running the algorithm. </summary> */
	override public void Initiate_algorithm() {

		//initialize GAGenome to find out the length of binary strings
		GAGenome.Initialize();
		
		//set up the arrays
		currentGeneration = new GAGenome[MEMBERS];
		
		//randomly generate first generation
		for (int i = 0; i < MEMBERS; i++) {
			currentGeneration[i] = new GAGenome(); //create a random genome
		}

		//begin the update loop!
		StartCoroutine(Time_flock());
	}

	/** <summary>
	 * Stops the algorithm and ends the life of the current flock. </summary>  */
	override public void Stop_algorithm() {

		//TODO really bad way of doing this
		GACoefficient.TotalLength = 0;

		//perform general stopping procedures
		base.Default_stop ();

		//kill off the flock
		Stop_current_flock();
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * A coroutine (see <see cref="IEnumerator"/>) that runs the logic of the entire algorithm. </summary>  */
	IEnumerator Time_flock() {

		float fitnessViewTime = 2;
		float generationViewTime = 2;

		//repeat this for the total number of generations
		for (int i = 0; i < GENERATIONS; i++) {

			//reset the total fitness
			totalFitness = 0;

			//add the default display to the GUI
			base.AddDisplay("GADisplay", true);

			//repeat this for each member in the generation
			for (int j = 0; j < MEMBERS; j++) {
				
				//begin the new flock
				Start_new_flock(currentGeneration[j]);
				
				//wait for the run time to be over of the flock
				yield return new WaitForSeconds(RUN_TIME);

				//calculate the flock's fitness
				currentGeneration[j].Fitness = CalculateFitness();
				totalFitness += currentGeneration[j].Fitness;

				//wait another 2s so that the user can observe the fitness score
				yield return new WaitForSeconds(fitnessViewTime);

				//kill the flock
				Stop_current_flock();
			}

			//at the end of a generation:

			//switch out the display
			base.RemoveDisplay("GADisplay");
			base.AddDisplay("GAGenerationDisplay", true);

			//give the user 2s to view the generation's results
			yield return new WaitForSeconds(generationViewTime);

			//switch out the display again
			base.RemoveDisplay("GAGenerationDisplay");

			//sort the currentGeneration array by fitness
			OrderByFitness();

			//fill the currentGeneration array with new genomes
			currentGeneration = Breed ();
		}

		//TODO display scores ?? what do we want to display here?

		Stop_algorithm();
	}
	
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Creates a new flock, instantiating it, populating it, changes the camera target, and assigns a <see cref="VectorSet"/>. 
	 * Most of this is done outside the class. </summary>
	 * <param name="genome"> The <see cref="GAGenome"/> for the new flock </param> */
	private void Start_new_flock(GAGenome genome) {

		//create a new flock. base takes care of everything
		flock = base.Create_flock(Prefab.GA_FLOCK_PREFAB, new GAVectorSet(genome), NUM_BOIDS);

		//increment membersCounter
		membersCounter++;

		//notify all listeners that there's a new flock and information has been updated
		Notify ();
	}

	/** <summary>
	 * Ends the life of the current flock. </summary> */
	private void Stop_current_flock() {
		if (flock != null)	flock.GetComponent<GAFlock>().End_life();
	}
	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Creates a new generation of binary strings. This involves preserving elites,
	 * selecting mates, crossing over genomes, and mutating offspring. This method assumes that
	 * the currentGeneration's genomes are in order from greatest to least fitness score. </summary>
	 * <returns> A generation bred from the current generation </returns> */
	private GAGenome[] Breed() { 

		//array for the next generation.
		GAGenome[] nextGeneration = new GAGenome[MEMBERS];
		string[] mates = new string[2], offspring = new string[2];

		int arrayPlace;

		//carry the elites over into the new generation
		for (arrayPlace = 0; arrayPlace < ELITES; arrayPlace++) 
			nextGeneration[arrayPlace] = new GAGenome(currentGeneration[arrayPlace].BinaryString);

		//perform crossovers
		//if (MEMBERS - ELITES - RANDOM_MEMBERS) / 2 is not a whole number, a random genome will also be generated later
		for (int i = 0; i < ((MEMBERS - ELITES - RANDOM_MEMBERS) / 2); i++, arrayPlace += 2) {

			//make the selection. Options: Roulette, Rank, 
			mates = Selection(ROULETTE_SELECTION);

			//cross the genomes
			offspring = CrossGenomes(mates, 1);
			
			//mutate the offspring
			offspring[0] = Mutate (offspring[0]);
			offspring[1] = Mutate (offspring[1]);
			
			//save the offspring to the next generation array
			nextGeneration[arrayPlace] = new GAGenome(offspring[0]);
			nextGeneration[arrayPlace + 1] = new GAGenome(offspring[1]);
		}

		//fill the remainder with random new genomes
		while (arrayPlace < MEMBERS) 
			nextGeneration[arrayPlace++] = new GAGenome();

		//replace the currentGeneration
		return nextGeneration;
	}

	/** <summary>
	 * Selects two parents using the specified method. </summary>
	 * <param name="method"> The method to use </param> */
	private string[] Selection(int method) {

		string[] mates = new string[2];
		float denominator = 1, numerator = 1;

		//figure out the denominator
		if (method == ROULETTE_SELECTION)
			denominator = totalFitness;

		else if (method == RANK_SELECTION)
			denominator = (MEMBERS * (MEMBERS + 1)) / 2; // n * (n + 1) / 2

		//pick each mate
		for (int j = 0; j < 2; j++) {
			
			float crossoverCounter = 0;
			float crossoverDetermination = Random.Range (0, 1f);
			
			//look through the population to find a new mate
			for (int k = 0; k < MEMBERS; k++) {

				//figure out the numerator
				if (method == ROULETTE_SELECTION) numerator = currentGeneration[k].Fitness;
				else if (method == RANK_SELECTION) numerator = MEMBERS - k;

				//add the normalized fitness scores
				crossoverCounter += numerator / denominator;
				
				//if it wins or if it's the last genome in the array
				if (crossoverCounter >= crossoverDetermination || k == MEMBERS - 1) {
					
					//claim this as a mate
					mates[j] = currentGeneration[k].BinaryString;
					
					//break the loop
					k = MEMBERS;
				}
			}
		}
		return mates;
	}

	/** <summary>
	 * Crosses the two genome strings at the specified spots and returns the two children. </summary>
	 * <param name="binaries"> The genomes of the two parents </param>
	 * <param name="numSplits"> The number of places to split the genome </param>
	 * <returns> The two children generated by the crossing </returns> */
	private string[] CrossGenomes(string[] binaries, int numSplits) {

		string[] children = new string[] {"", ""};

		//randomly generate the places at which to split the genome
		int[] substringLengths = new int[numSplits];
		for (int i = 0; i < numSplits; i++)	substringLengths[i] = (int)((Random.Range(0f, (1f/numSplits))) * GAGenome.LENGTH);

		int stringSpot = 0;

		//now build the children with the substrings, alternating between them
		for (int j = 0; j < numSplits; j++) {
			children[0] += binaries[j % 2].Substring(stringSpot, substringLengths[j]);
			children[1] += binaries[(j + 1) % 2].Substring (stringSpot, substringLengths[j]);
			stringSpot += substringLengths[j];
		}

		//put the final substrings onto the children
		children[0] += binaries[numSplits % 2].Substring (stringSpot);
		children[1] += binaries[(numSplits + 1) % 2].Substring (stringSpot);

		//return the children
		return children;
	}

	/** <summary>
	 * Mutates a binary string and returns the new string using the <see cref="MUTATION_RATE"/>. </summary>
	 * <param name="binaryString"> The binary string to mutate </param>
	 * <returns> The mutated binary string </returns> */
	private string Mutate(string binaryString) {

		char[] stringChars = binaryString.ToCharArray();
		
		//for each character in the binary string
		for (int i = 0; i < binaryString.Length; i++) {
			
			//if the random number is less than the mutation chance
			if (Random.Range (0f, 1f) <= MUTATION_RATE) {

				//flip the bit. this has been tested
				stringChars[i] = char.Parse(((int.Parse(stringChars[i].ToString ()) + 1) % 2).ToString ());
			}
		}
		
		return new string(stringChars);
	}

	/*-------------------------------------------------------------------------------------------------------------*/

	/** <summary>
	 * Calculate the final fitness of a flock. </summary> */
	private float CalculateFitness() { //TODO This needs to be much better IMPORTANT
		
		GAFlock flockScript = flock.GetComponent<GAFlock>();
		float fitness = 0;
		float goalDistance = 7f;
		float goalSpeed = (Flock.MIN_SPEED + Flock.MAX_SPEED) / 2;
		
		//get the variables used to calculate the fitness from the flock
		int colliderCount = flockScript.Num_Collisions;
		float averageDistance = flockScript.Average_Distance;
		
		//forgive one or two collisions
		if (colliderCount <= 3) colliderCount = 0;
		else colliderCount -= 3;
		
		//calculate the fitness
		fitness = (1/(Mathf.Abs (goalDistance - averageDistance) * Mathf.Abs (goalSpeed - flock.rigidbody.velocity.magnitude)))/(1 + colliderCount * 0.01f);
		
		return fitness;
	}

	/** <summary>
	 * Order the population members by their fitness level. Uses bubble sort. </summary> */
	private void OrderByFitness() { 
		
		GAGenome temp = null;

		//order the array
		for (int write = 0; write < currentGeneration.Length; write++) {
			
			//for each subsequent fitness
			for (int sort = 0; sort < currentGeneration.Length - 1; sort++) {
				
				if (currentGeneration[sort].Fitness < currentGeneration[sort + 1].Fitness) {

					temp = currentGeneration[sort + 1];
					currentGeneration[sort + 1] = currentGeneration[sort];
					currentGeneration[sort] = temp;
				}
			}
		}
	}

	/*-------------------------------------------------------------------------------------------------------------*/
	
	/** <summary> 
	 * Returns the <see cref="GAGenome"/> of the flock currently active. </summary> */
	public GAGenome Get_current_genome() {
		return currentGeneration[membersCounter % MEMBERS];
	}

	/** <summary> 
	 * The total fitness of the generation. </summary> */
	public float AverageFitness {
		get { return totalFitness / ((membersCounter % MEMBERS) + 1); }
	}

	/** <summary> 
	 * The total fitness of the generation. </summary> */
	public float Flock_Speed {
		get { return flock.rigidbody.velocity.magnitude; }
	}

	/** <summary> 
	 * The number of collisions that have occurred within the current flock. </summary> */
	public int Collisions {
		get { return flock.GetComponent<GAFlock>().Num_Collisions; }
	}
	
	/** <summary>
	 * The current generation. </summary>  */
	public int Generation {
		get { return (membersCounter / MEMBERS) + 1; }
	}

	/** <summary>
	 * The number of the flock in the current generation. </summary>  */
	public int Member_Number {
		get { return (membersCounter % MEMBERS); }
	}
}
