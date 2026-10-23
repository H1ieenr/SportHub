export interface OptionValue {
  id: number;
  option_definition_id: number;
  value: string;
  display_order: number;
}

export interface CreateOptionValueItem {
  value: string;
  display_order: number;
}

export interface CreateOptionValuePayload {
  option_definition_id: number;
  create_option_values: CreateOptionValueItem[];
}

export interface UpdateOptionValuePayload {
  id: number;
  value: string;
  display_order: number;
}