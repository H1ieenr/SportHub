import { CreateOptionValueItem, OptionValue } from "../option-value/option-value.model";

export interface OptionDefinition {
  id: number;
  code: string;
  name: string;
  is_active: boolean;
  option_values: OptionValue[];
}

export interface OptionDefinitionQuery {
  search_text?: string;
  code?: string;
  ids?: number[];
}

export interface CreateOptionDefinitionPayload {
  code: string;
  name: string;
  create_option_values?: CreateOptionValueItem[];
}

export interface UpdateOptionDefinitionPayload
  extends CreateOptionDefinitionPayload {
  id: number;
  is_active: boolean;
}