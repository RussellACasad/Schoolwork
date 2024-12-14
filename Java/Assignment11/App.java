package com.example;

import javafx.application.Application;
import javafx.geometry.HPos;
import javafx.geometry.Insets;
import javafx.geometry.Pos;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.Label;
import javafx.scene.control.Slider;
import javafx.scene.control.TextArea;
import javafx.scene.input.Clipboard;
import javafx.scene.layout.ColumnConstraints;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.GridPane;
import javafx.scene.layout.Priority;
import javafx.stage.Stage;
import java.io.IOException;
import javafx.scene.input.ClipboardContent;

/**
 * JavaFX App
 */
public class App extends Application {

    private Boolean isDeleteClicked = false; 
    private boolean didCopy = false; 

    @Override
    public void start(Stage stage) throws IOException {
        /* Variable Declaration */
        FlowPane flowPane = new FlowPane(); //One GridPane and one FlowPane layout.
        GridPane sliderPane = new GridPane(); 
        GridPane gridPane = new GridPane(); 
        GridPane topGrid = new GridPane(); 
        GridPane outputTitleGrid = new GridPane(); 
        Label inputLabel = new Label("Input Text:");
        Label outputLabel = new Label("Output Text:");
        Label sliderLabel = new Label("Shift: 5"); 
        TextArea inputArea = new TextArea();
        TextArea outputArea = new TextArea();
        Slider slider = new Slider(1, 25, 5);
        Button encryptButton = new Button("Encrypt");
        Button clearButton = new Button("Clear All");
        Button decryptButton = new Button("Decrypt");
        Button copyButton = new Button("Copy");
        Button pasteButton = new Button("Paste");
        ColumnConstraints growGridConstraint = new ColumnConstraints(); 
        ColumnConstraints fixedTitleButton = new ColumnConstraints(); 
        Clipboard clipboard = Clipboard.getSystemClipboard(); 
        ClipboardContent content = new ClipboardContent(); 

        /* Pane Setup */
        gridPane.setPadding(new Insets(10)); //The GridPane should have insets of 10, 10, 10, 10.
        gridPane.setVgap(5);//The GridPane should have a vertical gap of 5.
        flowPane.setPadding(new Insets(10));//The FlowPane should have insets of 10, 10, 10, 10.
        flowPane.setAlignment(Pos.CENTER);//The FlowPane alignment should be CENTER.
        flowPane.setHgap(5);//The FlowPane should have a horizontal gap of 5.
        flowPane.setVgap(5);
        growGridConstraint.setHgrow(Priority.ALWAYS); // Modifies the top grid, which contains title and the clear button so the clear button can be right aligned
        fixedTitleButton.setMinWidth(80);
        fixedTitleButton.setMaxWidth(80);
        topGrid.setHgap(5);
        topGrid.getColumnConstraints().addAll(growGridConstraint, fixedTitleButton, fixedTitleButton);
        gridPane.getColumnConstraints().addAll(growGridConstraint);
        gridPane.minHeight(500);
        gridPane.minWidth(250); 
        outputTitleGrid.getColumnConstraints().addAll(growGridConstraint, growGridConstraint);

        /* Adding Elements to parents */
        
        //Row 0: Title and Clear button
        gridPane.add(topGrid, 0, 0);//A Label for the text "Input Text:" should be placed on the GridPane in location column 0, row 0.
        topGrid.add(inputLabel, 0, 0);     

        //Row 1: Input box
        gridPane.add(inputArea, 0, 1); //A TextArea should be placed on the GridPane in location column 0, row 1.
    
        // Row 2: Slider for cipher shift
        slider.valueProperty().addListener(actionEvent -> 
        {
            slider.setValue(Math.round(slider.getValue())); // snaps the slider to the nearest whole number to prevent decimals
            sliderLabel.setText("Shift: " + String.valueOf((int)slider.getValue())); // changes the label so the user knows the selected number
        });
        sliderPane.setAlignment(Pos.CENTER); // centers the slider
        sliderPane.add(sliderLabel, 0, 0); // adds the label to the slider pane
        sliderPane.add(slider, 1, 0); // adds the slider to the slider pane
        sliderPane.getColumnConstraints().add(new ColumnConstraints(50)); // sets the label to always have a width of 50
        gridPane.add(sliderPane, 0, 2); // adds the slider to row 2

        // Row 3: Encrypt/Decrypt buttons
        //A Button with the text "Encrypt" and a Button with the text "Clear" and a Button with the text "Decrypt" should be added to the FlowPane.
        encryptButton.setOnAction(actionEvent -> { outputArea.setText(encrypt(inputArea.getText(), (int)slider.getValue())); });// Encrypt Button
        pasteButton.setOnAction(actionEvent -> 
        {
            inputArea.setText(clipboard.getString());
        });
        topGrid.add(pasteButton, 1, 0);
        clearButton.setOnAction(actionEvent -> // Clear button, does a confirm to ensure deletion is wanted.
        { 
            if(isDeleteClicked) // if pressed twice
            {
                outputArea.setText(""); 
                inputArea.setText(""); // clears both text boxes
                isDeleteClicked = false; // resets the button
                clearButton.setText("Delete All");
                clearButton.getStyleClass().set(1, "clearButton");
            } 
            else // if pressed once
            {
                isDeleteClicked = true; 
                clearButton.setText("Confirm");
                clearButton.getStyleClass().set(1, "clearButtonWarn");
            }
        });
        clearButton.hoverProperty().addListener(actionEvent -> // if the user stops hovering the "confirm" option
        {
            if(isDeleteClicked && !clearButton.isHover())
            {
                clearButton.setText("Delete All");
                isDeleteClicked = false; 
                clearButton.getStyleClass().set(1, "clearButton"); 
            } 
        });
        topGrid.add(clearButton, 2, 0);// adds clear button to top grid
        GridPane.setHalignment(clearButton, HPos.RIGHT); // Right aligns the clear button
        decryptButton.setOnAction(actionEvent -> { outputArea.setText(decrypt(inputArea.getText(), (int)slider.getValue())); }); // Decrypt Button
        flowPane.getChildren().addAll(encryptButton, decryptButton); // adds encrypt / decrypt button to the flowpane
        gridPane.add(flowPane, 0, 3); //The FlowPane should be placed on the GridPane in location column 0, row 3.
        
        // Row 4: Output label and copy button
        outputTitleGrid.add(outputLabel, 0, 0);
        gridPane.add(outputTitleGrid, 0, 4); //A Label with the text "Output Text" should be placed on the GridPane in location column 0, row 4.
        copyButton.setOnAction(actionEvent -> 
        {
            content.putString(outputArea.getText());
            clipboard.setContent(content); 
            didCopy = true; 
            copyButton.setText("Copied!"); 
            copyButton.getStyleClass().set(1, "copyButtonCopied");
        });
        copyButton.hoverProperty().addListener(actionEvent -> 
        {
            if(didCopy)
            {
                didCopy = false; 
                copyButton.setText("Copy"); 
                copyButton.getStyleClass().set(1, "copyButton"); 
            }
        });
        GridPane.setHalignment(copyButton, HPos.RIGHT);
        outputTitleGrid.add(copyButton, 1, 0);

        // Row 5: Output Box
        gridPane.add(outputArea, 0, 5); //A TextArea should be placed on the GridPane in location column 0, row 5.
        
        Scene scene = new Scene(gridPane, 500, 500); // Sets the scene with a width and height of 500

        /* STYLES - Imports the style sheet and adds classes to elements*/
        scene.getStylesheets().add(this.getClass().getResource("main.css").toExternalForm());
        clearButton.getStyleClass().add("clearButton");
        gridPane.getStyleClass().add("main");
        inputArea.getStyleClass().add("textbox");
        outputArea.getStyleClass().add("textbox");
        inputLabel.getStyleClass().add("titleLabel");
        outputLabel.getStyleClass().add("titleLabel");
        copyButton.getStyleClass().add("copyButton");
        pasteButton.getStyleClass().add("copyButton");


        // Sets the title of the application
        stage.setTitle("CPT-236 Encryption Applicaiton");
        stage.setScene(scene);
        stage.show();
    }

    public String encrypt(String input, int shift)
    {
        var output = "";

        for (var x : input.toCharArray()) {
            if(x == ' ')
            {
                output += Character.toString(x);
            } else {
				int y;

                if(x <= '9' && x >= '0')
                {
                    y = x; 
                    for (var i = 0; i < shift; i++)
                    {
                        y = (y =='9') ? '0' : y + 1; 
                    }
                    output += Character.toString(y);
                    continue; 
                }
				else 
                {
                    y = x + shift; // shifts the letter by the inputted number
                }

                System.out.println(Character.toString(y));
                System.out.println(y > '9' && x <= '9' && x >= '0');
                System.out.println((y - '9') > '9');
                System.out.println((y - '9') * 2);
                if(y > 'z' && x <= 'z' && x >= 'a') // if the shift extends past the alphabet, loops the encoding to ensure only alphabetic characters are used.
                {
                    y = (char)(('a' - 1) + (y - 'z'));
                }
                else if(y > 'Z' && x <= 'Z' && x >= 'A')
                {
                    y = (char)(('A' - 1) + (y - 'Z'));
                }
                output += Character.toString(y);
            }
        }
        return output; // returns the shifted string
    }

    public String decrypt(String input, int shift)
    {
        var output = ""; 
        for (var x : input.toCharArray()) {
            if(x == ' ') // spaces don't get shifted
            {
                output += x;
            } else {
				int y; // shifts the letter back to how it should be
				if(x <= '9' && x >= '0')
                {
                    y = x; 
                    for (var i = 0; i < shift; i++)
                    {
                        y = (y =='0') ? '9' : y - 1; 
                    }
                    output += Character.toString(y);
                    continue; 
                }
				else 
                {
                    y = x - shift; // shifts the letter by the inputted number
                } 

                if(y < 'a' && x <= 'z' && x >= 'a') // loops the letter if needbe
                {
                    y = (char)(('z' + 1) - ('a' - y));
                }
                else if(y < 'A' && x <= 'Z' && x >= 'A')
                {
                    y = (char)(('Z' + 1) - ('A' - y));
                }
                output += Character.toString(y);
            }
        }
        return output;
    }

    public static void main(String[] args) {
        launch();
    }

}