# Simple Genetic Algorithm

SimpleGeneticAlgorithm is an implementation of a genetic algorithm in C#. Genetic algorithms are inspired by the process of natural selection, where the fittest individuals are selected for reproduction to produce the offspring of the next generation. This project aims to provide a simple, yet efficient example of how a genetic algorithm might be applied.

## Table of Contents

1. [Overview](#Overview)
2. [Features](#Features)
3. [Example](#Example)

## Overview

A genetic algorithm is a search heuristic that is inspired by Charles Darwin’s theory of natural evolution. It reflects the process of natural selection where the fittest individuals are selected for reproduction in order to produce the offspring of the next generation. 

The algorithm starts with an initial set of random solutions, or 'population'. Each individual solution is evaluated based on a fitness function. The fittest individuals are then chosen to create new offspring through operations mimicking natural genetics: crossover (or recombination), and mutation. The new generation of solutions is then used for the next iteration of the algorithm. The process continues until a satisfactory solution is discovered, or the maximum number of generations is reached.

## Features

- Implementation of genetic algorithm operations including selection, crossover (recombination), and mutation.
- Made with C# making it powerful, yet easy to understand and use.
- Detailed example showing the practical application of the algorithm.
- Dynamic visualization of the algorithm's process.

## Example

In the `Example` directory, there's a detailed sample application of the genetic algorithm. It demonstrates the workings of the algorithm in an understandable and visual manner.

The example application uses the genetic algorithm to form a sentence starting from a random sequence of characters. The characters are treated as genes, with each gene being a character. Over generations of selection, crossover, and mutation, the population of random characters gradually evolves to form the target sentence: "This is an example to calculate multiple characters that make up a sentence using a GENETIC ALGORITHM!!!". The fitness of each individual is calculated based on its closeness to this target sentence.

There's also a visualization that showcases how the algorithm functions. You can see it below:

![Genetic Algorithm Example](./simple_genetic_algorithm_example.gif)

This image demonstrates the algorithm's progression and how it finds solutions over time.
