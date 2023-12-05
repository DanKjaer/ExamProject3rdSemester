import {Injectable} from "@angular/core";
import {AnimalFeed, Animals, AnimalSpecies, AnimalSpeciesFeed} from "./models";

@Injectable({
  providedIn: 'root'
})

export class State{
  animalSpeciesFeed: AnimalSpeciesFeed[] = [];
  currentAnimalSpecies: AnimalSpecies = new AnimalSpecies();
  animalFeed: AnimalFeed[] = [];
  currentAnimal: Animals = new Animals();
}
