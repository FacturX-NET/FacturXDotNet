/**
 * Represents dates with values ranging from January 1, 0001 Anno Domini (Common Era) through December 31, 9999 A.D. (C.E.) in the Gregorian calendar.
 */
export interface DateOnly {
  /**
   * The day of the month (1-31).
   */
  day: number;
  /**
   * The month of the year (1-12).
   */
  month: number;
  /**
   * The year (4 digits).
   */
  year: number;
}
