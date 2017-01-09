package platform;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.Date;
import java.util.LinkedHashMap;
import java.util.Map;

import org.json.simple.JSONObject;
import org.json.simple.parser.JSONParser;
import org.json.simple.parser.ParseException;

import chart.lineChart;
import cvsReader.ReadCVS;

/* 
 *  Main trading platform. All inputs & outputs pass through here. 
 * 
 */
public class Platform {
	
	public static void main(String[] args) {
		
		
		String TickerURL = "https://www.bitstamp.net/api/ticker/";
		String Response = null;
		Map TickerData = null;
		
		// load historical bitcoin data
		ReadCVS reader = new ReadCVS();
		LinkedHashMap<Date, Double> history = reader.run();
		lineChart chart = new lineChart(
			      "BTC Price" ,
			      "BTCUSD", history);
		
		while(true) {	
			// check bitstamp api
			try {
				Response = httpGet(TickerURL);
	
			} catch (IOException e) {
				// TODO Auto-generated catch block
				System.out.println("API broke");
				e.printStackTrace();
			} 
			
			//get response string and parse to map
			TickerData = parse(Response);
			System.out.println(TickerData);
			
			//convert timestamp to date
			java.util.Date time=new java.util.Date((long)Integer.valueOf(Integer.valueOf(TickerData.get("timestamp").toString()))*1000);
			history.put(time, Double.valueOf(TickerData.get("last").toString()));
			
			
			try {
				// Sleep for a while.
			    Thread.sleep(3000);                 //1000 milliseconds is one second.
			} catch(InterruptedException ex) {
			    Thread.currentThread().interrupt();
			}
		}
	}
	
	// Returns a string from bitstamp api containing all current bitcoin information
	public static String httpGet(String urlStr) throws IOException {
		  URL url = new URL(urlStr);
		  HttpURLConnection conn =
		      (HttpURLConnection) url.openConnection();

		  if (conn.getResponseCode() != 200) {
		    throw new IOException(conn.getResponseMessage());
		  }

		  // Buffer the result into a string
		  BufferedReader rd = new BufferedReader(
		      new InputStreamReader(conn.getInputStream()));
		  StringBuilder sb = new StringBuilder();
		  String line;
		  while ((line = rd.readLine()) != null) {
		    sb.append(line);
		  }
		  rd.close();

		  conn.disconnect();
		  return sb.toString();
		}
	
	//Parses a JSON string and returns a map
	public static Map parse(String string) {
		
		JSONParser parser = new JSONParser();
		
		Map<Date, Double> map = null;
		
		try {
			JSONObject jsonObject = (JSONObject) parser.parse(string);
				
			//System.out.println(jsonObject.get("timestamp") + "," + jsonObject.get("last"));
			map = jsonObject;	
			
		}catch(ParseException pe) {
			System.out.println("Something went wrong parsing..");
		}
		
		return map;
		
	}
}

