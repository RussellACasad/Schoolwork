#!/usr/bin/python3

# saveLoad programmed by Russell Casad
# This module includes the ability to save to and load from a csv file
# When loading from the file, it returns a list.

import csv


# saves an inputted list to a file name, also inputted by programmer
def save(file_name, List):
    with open(file_name, "w", newline="") as file:
        writer = csv.writer(file)
        writer.writerows(List)


# loads a csv file and returns a list
def load(file_name):
    list_to_load = []

    # attempts to open file, if it doesn't exist, it creates an empty file
    try:
        open(file_name)
    except FileNotFoundError:
        save(file_name, list_to_load)

    # opens the newly created file, and returns a list
    with open(file_name, "r", newline="") as file:
        reader = csv.reader(file)
        for row in reader:
            list_to_load.append(row)
        return list_to_load
