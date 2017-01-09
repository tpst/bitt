package chart;

import java.awt.BorderLayout;
import java.awt.Color;
import java.awt.Font;
import java.util.Date;
import java.util.Map;

import javax.swing.JPanel;
import javax.swing.JTextField;

import org.jfree.chart.ChartFactory;
import org.jfree.chart.ChartPanel;
import org.jfree.chart.JFreeChart;
import org.jfree.data.category.DefaultCategoryDataset;
import org.jfree.data.time.Day;
import org.jfree.data.time.TimeSeries;
import org.jfree.data.time.TimeSeriesCollection;
import org.jfree.data.xy.XYDataset;
import org.jfree.data.xy.XYSeries;
import org.jfree.data.xy.XYSeriesCollection;
import org.jfree.ui.ApplicationFrame;
import org.jfree.ui.RefineryUtilities;
import com.tictactec.ta.lib.*;

public class lineChart extends ApplicationFrame
{
   
	public TimeSeriesCollection dataset = null;
	

	public lineChart(String applicationTitle, String chartTitle, Map<Date, Double> map) {
		super(applicationTitle);
		  
		// Fills dataset with btc history information
		dataset = createDataset(map);
		  System.out.println(dataset.getItemCount(0));

		setLayout(new BorderLayout(5, 5));

		  
	    JFreeChart lineChart = ChartFactory.createTimeSeriesChart(
	    	         chartTitle,
	    	         "Date","BitStampUSD",
	    	         dataset,
	    	         true,true,false);
	    ChartPanel chartPanel = new ChartPanel( lineChart );
	    chartPanel.setPreferredSize( new java.awt.Dimension( 560 , 375 ) );
	      //setContentPane( chartPanel );
	    getContentPane().add(chartPanel, BorderLayout.NORTH);
	    
	    addStochRSI(dataset);
	    
	      this.pack( );
	      RefineryUtilities.centerFrameOnScreen( this );

	      this.setVisible( true );
	      
	      
		
	}
	
	private void addStochRSI(TimeSeriesCollection dataset) {
		/* JPanel p = new JPanel();
	        JTextField console = new JTextField("Console:", 40);
	        console.setFont(new Font("SanSerif", Font.BOLD, 16));
	        console.setEditable(false);
	        console.setBackground(Color.WHITE);
	        p.add(console);
		    this.getContentPane().add(p, BorderLayout.SOUTH);*/
		
		  int TOTAL_PERIODS = dataset.getItemCount(0);
		  
		  double[] _dataset = new double[dataset.getItemCount(0)];
		    /**
		     * The number of periods to average together.
		     */
		    double[] closePrice = new double[TOTAL_PERIODS];
	        double[] out = new double[TOTAL_PERIODS];
	        double[] outFastK = new double[TOTAL_PERIODS];
	        double[] outFastD = new double[TOTAL_PERIODS];
	        MInteger begin = new MInteger();
	        MInteger length = new MInteger();

	        for (int i = 0; i <= dataset.getItemCount(0)-1; i++) {
	            _dataset[i] = dataset.getYValue(0, i);
	        }

	        Core c = new Core();
	        //RetCode retCode = c.sma(0, closePrice.length - 1, closePrice, PERIODS_AVERAGE, begin, length, out);
	        RetCode retCode = c.stochRsi(0, closePrice.length-1, _dataset, 14, 14, 3, MAType.Sma, begin, length, outFastK, outFastD);
	        //RetCode retCode = c.stochRsi(startIdx, endIdx, inReal, optInTimePeriod, optInFastK_Period, optInFastD_Period, optInFastD_MAType, outBegIdx, outNBElement, outFastK, outFastD)
	        if (retCode == RetCode.Success) {
	            System.out.println("Output Begin:" + begin.value);
	            System.out.println("Output End:" + length.value);
	            TimeSeriesCollection stochRSI = new TimeSeriesCollection();
                TimeSeries fastK = new TimeSeries("%K");
                TimeSeries fastD = new TimeSeries("%D");
	            for (int i = 0; i <= dataset.getItemCount(0)-1; i++) {
/*	                StringBuilder line = new StringBuilder();
	                line.append("Period #");
	                line.append(i);
	                line.append(" %K: ");
	                line.append(outFastK[i]);
	                line.append(" %D: ");
	                line.append(outFastD[i]);
	                System.out.println(line.toString());*/
	                
	                fastK.add(dataset.getSeries(0).getTimePeriod(i), outFastK[i]);
	                fastD.add(dataset.getSeries(0).getTimePeriod(i), outFastD[i]);

	                
	            }
                stochRSI.addSeries(fastK);
                stochRSI.addSeries(fastD);
	            JFreeChart lineChart = ChartFactory.createTimeSeriesChart("StochRSI", "Date", "", stochRSI);
                ChartPanel chartPanel = new ChartPanel( lineChart );
        	    chartPanel.setPreferredSize( new java.awt.Dimension( 560 , 250 ) );
        	      //setContentPane( chartPanel );
        	    this.getContentPane().add(chartPanel, BorderLayout.SOUTH);
	        }
	        else {
	            System.out.println("Error");
	        }
		    
		
	}
	
	private void updateHistory() {
		
	}
	
	private TimeSeriesCollection createDataset(Map<Date, Double> map)
	   {
		   TimeSeries timeSeries = new TimeSeries("BTCUSD");
		  
		   XYSeriesCollection RSIseries = new XYSeriesCollection();
		   	 
		   //loop map
		   for (Map.Entry<Date, Double> entry : map.entrySet()) {
			   
			   Day newDay = new Day(entry.getKey());
			   	timeSeries.add(newDay, entry.getValue());
			   /*System.out.println("Timestamp" + entry.getKey() + " , Price "
					   + entry.getValue());*/
				   //System.out.println(dateKey + ", "+ entry.getValue());
			   
			

		   }

		  TimeSeriesCollection data = new TimeSeriesCollection(timeSeries);
		 /* for(int i = 0; i < series1.getItemCount(); i++) {
			  System.out.println(series1.getDataItem(i));
		  }*/
	      return data;
	   }

}
