using System;
using System.Collections;
using System.Collections.Generic;

using Machine.Specifications;

namespace Machine.UoW.Specs
{
  [Concern("Unit of work creation")]
  public class when_creating_a_new_unit_of_work
  {
    static UnitOfWorkFactory factory;
    static UnitOfWork uow;

    Establish context = () =>
    {
      factory = new UnitOfWorkFactory(new UnitOfWorkManagement());
    };

    Because of = () =>
    {
      uow = (UnitOfWork)factory.StartUnitOfWork();
    };

    It should_have_no_entries = () =>
      uow.Entries.ShouldBeEmpty();
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
