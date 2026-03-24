export enum UserRole {
  SuperAdmin = 'SuperAdmin',
  SchoolAdmin = 'SchoolAdmin',
  Principal = 'Principal',
  Teacher = 'Teacher',
  Student = 'Student'
}

export enum ParticipantType {
  IndividualStudent = 1,
  Section = 2,
  Teacher = 3,
  Department = 4
}

export const ParticipantTypeOptions = [
  { value: ParticipantType.IndividualStudent, label: 'Student' },
  { value: ParticipantType.Section, label: 'Section' },
  { value: ParticipantType.Teacher, label: 'Teacher' },
  { value: ParticipantType.Department, label: 'Department' }
];

export enum MeetingType {
  PTM = 1,
  SectionMeeting = 2,
  StaffMeeting = 3,
  AcademicMeeting = 4,
  ManagementMeeting = 5,
  GeneralMeeting = 6,
  EmergencyMeeting = 7
}

export const MeetingTypeOptions = [
  { value: MeetingType.PTM, label: 'PTM' },
  { value: MeetingType.SectionMeeting, label: 'Section Meeting' },
  { value: MeetingType.StaffMeeting, label: 'Staff Meeting' },
  { value: MeetingType.AcademicMeeting, label: 'Academic Meeting' },
  { value: MeetingType.ManagementMeeting, label: 'Management Meeting' },
  { value: MeetingType.GeneralMeeting, label: 'General Meeting' },
  { value: MeetingType.EmergencyMeeting, label: 'Emergency Meeting' }
];
