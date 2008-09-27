using System;
using System.Collections.Generic;

using Machine.Specifications;

namespace Machine.UoW.Specs
{
  [Concern("Unit of work creation")]
  public class when_creating_a_new_unit_of_work
  {
    It should_have_no_entries;
  }

  [Concern("Rolling back unit of work")]
  public class when_rolling_back_a_unit_of_work
  {
    It should_call_rollback_for_all_objects;
  }
  
  [Concern("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_added_objects
  {
    It should_call_addnew;
  }
  
  [Concern("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_saved_objects
  {
    It should_call_save;
  }
  
  [Concern("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_deleted_objects
  {
    It should_call_delete;
  }
  
  [Concern("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_added_and_then_deleted_objects
  {
    It should_call_nothing;
  }
  
  [Concern("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_saved_and_then_deleted_objects
  {
    It should_call_delete;
  }
  
  [Concern("Flushing unit of work")]
  public class when_flushing_a_unit_of_work_with_added_and_then_saved_objects
  {
    It should_call_addnew_and_then_save;
  }
}
