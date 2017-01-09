package cvsReader;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.Locale;

import org.apache.commons.io.input.ReversedLinesFileReader;

// Loads and reads bitcoin historical data from a cvs file
public class ReadCVS {

	  public static void main(String[] args) {

		ReadCVS obj = new ReadCVS();
		obj.run();

	  }
	  
	  public LinkedHashMap<Date, Double> run() {

	    File file = new File("Resources/per_day_all_time_history.csv");
		BufferedReader br = null;
		BufferedReader rl = null;
		String line = "";
		String cvsSplitBy = ",";
		LinkedHashMap<Date, Double> maps = new LinkedHashMap<Date, Double>();

		try {

			
			rl = new BufferedReader(new FileReader(file));
			
			int lineCount = 0;
			
			// NOTE: This currently only takes 30 lines to quicken testing
			
			while ((line = rl.readLine()) != null && (lineCount < 2000000)) {
				
				if(lineCount>0) {
					// use comma as separator
					String[] priceData = line.split(cvsSplitBy);
					//System.out.println(priceData[0] + "," + priceData[3]);
					
					DateFormat format = new SimpleDateFormat("yyyy-MM-dd", Locale.ENGLISH);
					Date date;
					try {
						date = format.parse(priceData[0]);
						//System.out.println(date); // Sat Jan 02 00:00:00 GMT 2010
						//Map each timestamp to a price [timestamp, price]
				
						//System.out.println(date + "::: " + priceData[1]);

						maps.put(date, Double.valueOf(priceData[3]));	
						//System.out.println(lineCount++);
	
					} catch (ParseException e) {
						System.out.println("Invalid timestamp");
						e.printStackTrace();
					}
				}
					
			lineCount++;
				
			}
			
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			if (br != null) {
				try {
					br.close();
					rl.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
		}
		return maps;
	  }

	}