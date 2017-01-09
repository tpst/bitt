package chart;

import java.awt.BorderLayout;

import javax.swing.JFrame;

public class chartPanelTest extends JFrame {

	public chartPanelTest() {
		getContentPane().setLayout(new BorderLayout(5, 5));
		setTitle("BTC USD");
		try {
			chartInit();
		} catch (Exception e) {
			e.printStackTrace();
		}
	
	}
	
	// Initialises main btc price chart
	private void chartInit() {
		
	}
	
	public static void main() {
		chartPanelTest newChart = new chartPanelTest();
		newChart.setVisible(true);
	}
	
}
