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
  animalBirthday?: Date;
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

export class LoginResponse{
  token!: string;
}

export class AnimalNote{
  animalID!: number;
  noteText!: string;
}

export class AnimalNoteFeed{
  noteID!: number;
  animalID!: number;
  noteText!: string;
  noteDate!: Date;
}
