using UnityEngine;
using System.Collections;
using System.IO;

/** <summary>
 * This class handles writing to files. </summary> */
public sealed class FileWriter {

	/** <summary>
	 * The name of the file that algorithm information is recorded to. </summary> */
	private static string fileName = "MyFile.txt";

	/** <summary>
	 * Writes a message to a specified text file. </summary>
	 * <param name="message"> The message to write to the file </param>
	 * <returns> Whether or not the operation was successful </returns> */
	public static void WriteToFile(string message) {

		System.IO.File.AppendAllText(fileName, message);
	}
}
