using System;
using System.Collections.Generic;

using Machine.Specifications;

namespace Machine.UoW.Specs
{
  [Concern("Unit of work")]
  public class when_adding_object
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_added_change;
  }

  [Concern("Unit of work")]
  public class when_adding_object_multiple_times
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_added_change;
  }

  [Concern("Unit of work")]
  public class when_saving_object
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_saved_change;
  }

  [Concern("Unit of work")]
  public class when_saving_object_multiple_times
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_saved_change;
  }

  [Concern("Unit of work")]
  public class when_deleting_object
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_deleted_change;
  }

  [Concern("Unit of work")]
  public class when_deleting_object_multiple_times
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_deleted_change;
  }

  [Concern("Unit of work")]
  public class when_adding_and_then_removing_object
  {
    It should_remove_the_object_from_the_entries;
  }

  [Concern("Unit of work")]
  public class when_saving_and_then_removing_object
  {
    It should_remove_the_object_from_the_entries;
  }

  [Concern("Unit of work")]
  public class when_deleting_and_then_removing_object
  {
    It should_remove_the_object_from_the_entries;
  }

  [Concern("Unit of work")]
  public class when_removing_object_that_has_no_entry
  {
    It should_fail;
  }

  [Concern("Unit of work")]
  public class when_adding_and_then_deleting_object
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_added_and_deleted_changes;
  }

  [Concern("Unit of work")]
  public class when_saving_and_then_deleting_object
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_saved_and_deleted_changes;
  }

  [Concern("Unit of work")]
  public class when_saving_and_then_adding_object
  {
    It should_fail;
  }

  [Concern("Unit of work")]
  public class when_adding_and_then_saving_and_then_deleting_object
  {
    It should_put_the_object_in_the_entries_once;
    It should_have_added_and_saved_and_deleted_changes;
  }

  [Concern("Unit of work")]
  public class when_deleting_and_then_saving_object
  {
    It should_fail;
  }

  [Concern("Unit of work")]
  public class when_deleting_and_then_adding_object
  {
    It should_fail;
  }
}
