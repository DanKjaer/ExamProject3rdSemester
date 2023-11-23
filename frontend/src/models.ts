export class AnimalSpeciesFeed{
  speciesID!: number;
  speciesName?: string;
  speciesPicture?: string;
}

export class AnimalSpecies{
  speciesID!: number;
  speciesName?: string;
  speciesDescription?: string;
  speciesPicture?: string;
}

export class Animals{
  animalID?: number;
  speciesID?: number;
  animalName?: string;
  animalGender?: boolean;
  animalDead?: boolean;
  animalPicture?: string;
  animalWeight?: number;
}

export class AnimalFeed{
  animalID!: number;
  speciesID!: number;
  animalName?: string;
}
