import {Injectable} from "@angular/core";
import {AnimalFeed, AnimalNote, AnimalNoteFeed, Animals, AnimalSpecies, AnimalSpeciesFeed} from "./models";

@Injectable({
  providedIn: 'root'
})

export class State{
  animalSpeciesFeed: AnimalSpeciesFeed[] = [];
  currentAnimalSpecies: AnimalSpecies = new AnimalSpecies();
  animalFeed: AnimalFeed[] = [];
  currentAnimal: Animals = new Animals();
  currentAnimalNote: AnimalNote = new AnimalNote();
  animalNoteFeed: AnimalNoteFeed[] = [];
}
