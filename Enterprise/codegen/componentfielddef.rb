require 'fielddef'
require 'type_name_utils'

# Represents the definition of a field that is a component
class ComponentFieldDef < FieldDef
  
  def initialize(model, fieldNode, defaultNamespace)
    super(model, fieldNode)
    @dataType = TypeNameUtils.getQualifiedName(fieldNode.attributes['class'], defaultNamespace)
  end

  def kind
    :component
  end
  
  def dataType
    @dataType
  end
  
  def initialValue
    "new #{dataType}()"
  end
  
  # a component field is mandatory if the component contains mandatory fields
  def isMandatory
    componentDef.mandatoryFields.length > 0
  end
  
  def supportDataType
    componentDef.supportClassName
  end
  
  def supportInitialValue
    "new #{componentDef.supportClassName}()"
  end

  def searchCriteriaDataType
    componentDef.searchCriteriaClassName
  end
  
  def searchCriteriaReturnType
    componentDef.searchCriteriaClassName
  end

protected
  def componentDef
    model.findClass(@dataType)
  end

end
