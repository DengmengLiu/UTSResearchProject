# [UTS Research Project] ChatGPT in Game Development -- Enhancing NPC Dialogues in Role-Playing Games Prototype

## Overview

This project explores the integration of advanced AI technology, specifically the ChatGPT API, to enhance non-player character (NPC) dialogue and interaction directly within the Unity game development environment. Developed as a project within the Unity Editor, it aims to streamline the development process and unlock new possibilities for creating richer and more dynamic in-game interactions.

## Key Features

* **AI-Driven NPC Integration:** Seamlessly integrate the ChatGPT API into the Unity Editor to create intelligent NPCs.
* **In-Editor Interaction:** Developers can interact with AI-powered NPCs directly within the Unity Editor using natural language input.
* **Customizable NPC Personalities:** Utilize system prompts to define NPC role backgrounds, behaviors, and dialogue styles, ensuring consistent and immersive characterization.
* **Knowledge Base Integration:** Leverages a PostgreSQL database with the `pgvector` extension to store and efficiently retrieve relevant NPC information (backstory, knowledge, dialogue snippets).
* **Context-Aware Responses:** Implements a similarity search mechanism (cosine similarity > 0.85) to fetch the most relevant information from the knowledge base based on developer input, providing ChatGPT with enhanced context for generating more appropriate and engaging responses.
* **Streamlined Development Workflow:** Facilitates rapid prototyping and iteration of NPC dialogues and interactions directly within the Unity Editor.

## Technologies Used

* **Unity Engine:** The primary game development platform.
* **ChatGPT API:** OpenAI's powerful language model for generating natural language responses.
* **PostgreSQL:** An open-source relational database used for storing NPC knowledge.
* **pgvector:** A PostgreSQL extension for efficient vector similarity search.
* **C#:** The primary programming language used in Unity.

## Intended Use

This project is designed to be used within the Unity Editor. Developers can:

1.  Access the custom tools or windows provided by this project.
2.  Select or create NPC entities within their Unity scene.
3.  Utilize the provided interface to interact with the selected NPC.
4.  Input natural language queries or statements.
5.  Observe the AI-generated responses from the ChatGPT-powered NPC within the editor environment.
6.  Use this functionality to test dialogue flows, refine NPC personalities, and quickly iterate on interactive elements without needing to build and run a full game demo.

While specific setup instructions will depend on your implementation, the general steps might involve:

1.  Obtain an API key from OpenAI for accessing the ChatGPT API.
2.  Set up a PostgreSQL database and install the `pgvector` extension.
3.  Configure the database connection details within your Unity project.
4.  Import the necessary scripts and assets into your Unity project's `Editor` folder.
5.  (Potentially) Create or import 3D models or other visual representations for your NPCs.
6.  Utilize the custom editor windows or components provided by this project to begin interacting with AI-powered NPCs.

## Potential Future Enhancements

* Dialog History Record and Embedding.
* Improve the Speed and EƯiciency of Database Searching Algorithms.
* Optimize User Interface (UI) / User Experience (UX) Design.
* Fine-Tuning ChatGPT Model.

## Project Duration

February 2024 - June 2024
