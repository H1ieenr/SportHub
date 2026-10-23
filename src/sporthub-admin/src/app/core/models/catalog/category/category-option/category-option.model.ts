import { OptionDefinition } from '../option-definition/option-definition.model';

export interface CategoryOptionItem {
  option_definition_id: number;
  display_order: number | null;
}

export interface CategoryOption {
  id: number;
  category_id: number;
  option_definition_id: number;
  display_order: number | null;
  option_definition: OptionDefinition;
}